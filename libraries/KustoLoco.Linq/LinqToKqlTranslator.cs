// Licensed under the MIT License.

using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace KustoLoco.Linq;

/// <summary>
/// Translates LINQ Expression trees to Kusto Query Language (KQL).
/// Based on the ExpressionVisitor pattern similar to Azure Cosmos DB LINQ provider.
/// </summary>
internal class LinqToKqlTranslator : ExpressionVisitor
{
    private readonly StringBuilder _kqlBuilder = new();
    private string? _tableName;
    private bool _inWhereClause;
    private bool _inSelectClause;
    private readonly List<string> _projections = new();
    private readonly List<string> _orderByExpressions = new();
    private int? _takeCount;
    private readonly Stack<string> _aggregations = new();

    public string Translate(Expression expression)
    {
        _kqlBuilder.Clear();
        _tableName = null;
        _projections.Clear();
        _orderByExpressions.Clear();
        _takeCount = null;

        Visit(expression);

        return BuildKqlQuery();
    }

    private string BuildKqlQuery()
    {
        var kql = new StringBuilder();

        // Start with table name or print if no table
        if (!string.IsNullOrEmpty(_tableName))
        {
            kql.Append(_tableName);
        }
        else if (_kqlBuilder.Length > 0)
        {
            return _kqlBuilder.ToString();
        }
        else
        {
            kql.Append("print result = 1");
        }

        // Add where clause
        if (_kqlBuilder.Length > 0)
        {
            kql.Append("\n| where ");
            kql.Append(_kqlBuilder);
        }

        // Add project clause
        if (_projections.Count > 0)
        {
            kql.Append("\n| project ");
            kql.Append(string.Join(", ", _projections));
        }

        // Add order by
        if (_orderByExpressions.Count > 0)
        {
            kql.Append("\n| sort by ");
            kql.Append(string.Join(", ", _orderByExpressions));
        }

        // Add take
        if (_takeCount.HasValue)
        {
            kql.Append("\n| take ");
            kql.Append(_takeCount.Value);
        }

        return kql.ToString();
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        // Handle LINQ method calls
        if (node.Method.DeclaringType == typeof(Queryable) || 
            node.Method.DeclaringType == typeof(Enumerable))
        {
            switch (node.Method.Name)
            {
                case "Where":
                    return VisitWhere(node);
                case "Select":
                    return VisitSelect(node);
                case "OrderBy":
                case "ThenBy":
                    return VisitOrderBy(node, ascending: true);
                case "OrderByDescending":
                case "ThenByDescending":
                    return VisitOrderBy(node, ascending: false);
                case "Take":
                    return VisitTake(node);
                case "Count":
                    return VisitCount(node);
                case "Sum":
                    return VisitAggregate(node, "sum");
                case "Average":
                    return VisitAggregate(node, "avg");
                case "Min":
                    return VisitAggregate(node, "min");
                case "Max":
                    return VisitAggregate(node, "max");
                case "First":
                case "FirstOrDefault":
                    return VisitFirst(node);
                case "Any":
                    return VisitAny(node);
                case "GroupBy":
                    return VisitGroupBy(node);
            }
        }

        // Handle string methods
        if (node.Method.DeclaringType == typeof(string))
        {
            return VisitStringMethod(node);
        }

        throw new NotSupportedException($"Method '{node.Method.Name}' is not supported in KQL translation.");
    }

    private Expression VisitWhere(MethodCallExpression node)
    {
        Visit(node.Arguments[0]); // Visit source

        var lambda = (LambdaExpression)StripQuotes(node.Arguments[1]);
        
        var previousInWhere = _inWhereClause;
        _inWhereClause = true;

        if (_kqlBuilder.Length > 0)
        {
            _kqlBuilder.Append(" and ");
        }

        Visit(lambda.Body);

        _inWhereClause = previousInWhere;

        return node;
    }

    private Expression VisitSelect(MethodCallExpression node)
    {
        Visit(node.Arguments[0]); // Visit source

        var lambda = (LambdaExpression)StripQuotes(node.Arguments[1]);

        var previousInSelect = _inSelectClause;
        _inSelectClause = true;

        // Handle different select scenarios
        if (lambda.Body is NewExpression newExpr)
        {
            // Select with anonymous type: Select(x => new { x.Name, x.Age })
            for (int i = 0; i < newExpr.Arguments.Count; i++)
            {
                var arg = newExpr.Arguments[i];
                var memberName = newExpr.Members?[i]?.Name;
                
                if (arg is MemberExpression memberExpr)
                {
                    _projections.Add(memberExpr.Member.Name + (memberName != memberExpr.Member.Name ? $" = {memberName}" : ""));
                }
                else
                {
                    // Complex expression
                    var exprStr = TranslateExpression(arg);
                    _projections.Add(memberName != null ? $"{memberName} = {exprStr}" : exprStr);
                }
            }
        }
        else if (lambda.Body is MemberExpression memberExpr)
        {
            // Select single property: Select(x => x.Name)
            _projections.Add(memberExpr.Member.Name);
        }
        else if (lambda.Body is ParameterExpression)
        {
            // Select the whole object: Select(x => x)
            // Don't add project clause
        }
        else
        {
            // Other expressions
            var exprStr = TranslateExpression(lambda.Body);
            _projections.Add(exprStr);
        }

        _inSelectClause = previousInSelect;

        return node;
    }

    private Expression VisitOrderBy(MethodCallExpression node, bool ascending)
    {
        Visit(node.Arguments[0]); // Visit source

        var lambda = (LambdaExpression)StripQuotes(node.Arguments[1]);
        
        var columnName = GetMemberName(lambda.Body);
        var orderExpr = ascending ? columnName : $"{columnName} desc";
        
        _orderByExpressions.Add(orderExpr);

        return node;
    }

    private Expression VisitTake(MethodCallExpression node)
    {
        Visit(node.Arguments[0]); // Visit source

        var countExpr = node.Arguments[1];
        if (countExpr is ConstantExpression constant)
        {
            _takeCount = (int)constant.Value!;
        }

        return node;
    }

    private Expression VisitCount(MethodCallExpression node)
    {
        Visit(node.Arguments[0]); // Visit source

        // If there's a predicate, handle it
        if (node.Arguments.Count > 1)
        {
            var lambda = (LambdaExpression)StripQuotes(node.Arguments[1]);
            _inWhereClause = true;
            Visit(lambda.Body);
            _inWhereClause = false;
        }

        var kql = BuildKqlQuery();
        _kqlBuilder.Clear();
        _kqlBuilder.Append(kql);
        _kqlBuilder.Append("\n| count");

        return node;
    }

    private Expression VisitAggregate(MethodCallExpression node, string aggregateFunction)
    {
        Visit(node.Arguments[0]); // Visit source

        string? columnName = null;
        if (node.Arguments.Count > 1)
        {
            var lambda = (LambdaExpression)StripQuotes(node.Arguments[1]);
            columnName = GetMemberName(lambda.Body);
        }

        var kql = BuildKqlQuery();
        _kqlBuilder.Clear();
        _kqlBuilder.Append(kql);
        _kqlBuilder.Append($"\n| summarize result = {aggregateFunction}({columnName ?? ""})");

        return node;
    }

    private Expression VisitFirst(MethodCallExpression node)
    {
        Visit(node.Arguments[0]); // Visit source

        if (node.Arguments.Count > 1)
        {
            // First with predicate
            var lambda = (LambdaExpression)StripQuotes(node.Arguments[1]);
            _inWhereClause = true;
            if (_kqlBuilder.Length > 0)
            {
                _kqlBuilder.Append(" and ");
            }
            Visit(lambda.Body);
            _inWhereClause = false;
        }

        _takeCount = 1;

        return node;
    }

    private Expression VisitAny(MethodCallExpression node)
    {
        Visit(node.Arguments[0]); // Visit source

        if (node.Arguments.Count > 1)
        {
            var lambda = (LambdaExpression)StripQuotes(node.Arguments[1]);
            _inWhereClause = true;
            Visit(lambda.Body);
            _inWhereClause = false;
        }

        var kql = BuildKqlQuery();
        _kqlBuilder.Clear();
        _kqlBuilder.Append(kql);
        _kqlBuilder.Append("\n| take 1\n| count");

        return node;
    }

    private Expression VisitGroupBy(MethodCallExpression node)
    {
        Visit(node.Arguments[0]); // Visit source

        var lambda = (LambdaExpression)StripQuotes(node.Arguments[1]);
        var groupByColumn = GetMemberName(lambda.Body);

        var kql = BuildKqlQuery();
        _kqlBuilder.Clear();
        _kqlBuilder.Append(kql);
        _kqlBuilder.Append($"\n| summarize count() by {groupByColumn}");

        return node;
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        if (_inWhereClause)
        {
            _kqlBuilder.Append("(");
            Visit(node.Left);
            
            _kqlBuilder.Append(" ");
            _kqlBuilder.Append(GetKqlOperator(node.NodeType));
            _kqlBuilder.Append(" ");
            
            Visit(node.Right);
            _kqlBuilder.Append(")");

            return node;
        }

        return base.VisitBinary(node);
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (_inWhereClause || _inSelectClause)
        {
            if (node.Expression is ParameterExpression)
            {
                _kqlBuilder.Append(node.Member.Name);
                return node;
            }
        }

        return base.VisitMember(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (_inWhereClause)
        {
            if (node.Value == null)
            {
                _kqlBuilder.Append("null");
            }
            else if (node.Value is string str)
            {
                _kqlBuilder.Append($"'{str.Replace("'", "\\'")}'");
            }
            else if (node.Value is bool b)
            {
                _kqlBuilder.Append(b ? "true" : "false");
            }
            else if (node.Value is DateTime dt)
            {
                _kqlBuilder.Append($"datetime({dt:yyyy-MM-dd HH:mm:ss})");
            }
            else if (IsNumericType(node.Value))
            {
                _kqlBuilder.Append(node.Value);
            }
            else if (node.Value is IQueryable)
            {
                // This is the root queryable, extract table name
                var type = node.Type;
                if (type.IsGenericType)
                {
                    var elementType = type.GetGenericArguments()[0];
                    _tableName = elementType.Name;
                }
            }
            
            return node;
        }

        // Handle table references
        if (node.Value is IQueryable)
        {
            var type = node.Type;
            if (type.IsGenericType)
            {
                var elementType = type.GetGenericArguments()[0];
                _tableName = elementType.Name;
            }
        }

        return node;
    }

    private Expression VisitStringMethod(MethodCallExpression node)
    {
        switch (node.Method.Name)
        {
            case "Contains":
                if (node.Object != null)
                {
                    Visit(node.Object);
                    _kqlBuilder.Append(" contains ");
                    Visit(node.Arguments[0]);
                }
                break;
            case "StartsWith":
                if (node.Object != null)
                {
                    Visit(node.Object);
                    _kqlBuilder.Append(" startswith ");
                    Visit(node.Arguments[0]);
                }
                break;
            case "EndsWith":
                if (node.Object != null)
                {
                    Visit(node.Object);
                    _kqlBuilder.Append(" endswith ");
                    Visit(node.Arguments[0]);
                }
                break;
            case "ToLower":
                _kqlBuilder.Append("tolower(");
                if (node.Object != null) Visit(node.Object);
                _kqlBuilder.Append(")");
                break;
            case "ToUpper":
                _kqlBuilder.Append("toupper(");
                if (node.Object != null) Visit(node.Object);
                _kqlBuilder.Append(")");
                break;
            default:
                throw new NotSupportedException($"String method '{node.Method.Name}' is not supported.");
        }

        return node;
    }

    private string GetKqlOperator(ExpressionType expressionType)
    {
        return expressionType switch
        {
            ExpressionType.Equal => "==",
            ExpressionType.NotEqual => "!=",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            ExpressionType.AndAlso => "and",
            ExpressionType.OrElse => "or",
            ExpressionType.Add => "+",
            ExpressionType.Subtract => "-",
            ExpressionType.Multiply => "*",
            ExpressionType.Divide => "/",
            ExpressionType.Modulo => "%",
            _ => throw new NotSupportedException($"Expression type '{expressionType}' is not supported.")
        };
    }

    private string GetMemberName(Expression expression)
    {
        if (expression is MemberExpression memberExpr)
        {
            return memberExpr.Member.Name;
        }

        if (expression is UnaryExpression unaryExpr)
        {
            return GetMemberName(unaryExpr.Operand);
        }

        throw new NotSupportedException($"Cannot extract member name from expression: {expression}");
    }

    private string TranslateExpression(Expression expression)
    {
        var savedBuilder = _kqlBuilder.ToString();
        _kqlBuilder.Clear();
        
        Visit(expression);
        
        var result = _kqlBuilder.ToString();
        _kqlBuilder.Clear();
        _kqlBuilder.Append(savedBuilder);
        
        return result;
    }

    private static Expression StripQuotes(Expression expression)
    {
        while (expression.NodeType == ExpressionType.Quote)
        {
            expression = ((UnaryExpression)expression).Operand;
        }
        return expression;
    }

    private static bool IsNumericType(object value)
    {
        return value is sbyte || value is byte || value is short || value is ushort ||
               value is int || value is uint || value is long || value is ulong ||
               value is float || value is double || value is decimal;
    }
}
