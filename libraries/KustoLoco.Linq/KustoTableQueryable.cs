// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace KustoLoco.Linq;

/// <summary>
/// Represents a queryable table in the Kusto context.
/// This is the starting point for LINQ queries against a specific table.
/// </summary>
/// <typeparam name="T">The type representing the table schema.</typeparam>
internal class KustoTableQueryable<T> : IOrderedQueryable<T>
{
    private readonly KustoQueryProvider _provider;
    private readonly string _tableName;

    public KustoTableQueryable(KustoQueryProvider provider, string tableName)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        
        // Create the initial expression representing the table
        Expression = Expression.Constant(this);
    }

    internal KustoTableQueryable(KustoQueryProvider provider, Expression expression)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        
        // Extract table name from the constant expression if this is a table source
        if (expression is ConstantExpression constantExpr && 
            constantExpr.Value is KustoTableQueryable<T> table)
        {
            _tableName = table._tableName;
        }
        else
        {
            _tableName = string.Empty;
        }
    }

    public Type ElementType => typeof(T);

    public Expression Expression { get; }

    public IQueryProvider Provider => _provider;

    internal string TableName => _tableName;

    public IEnumerator<T> GetEnumerator()
    {
        // Execute the query and return the results
        var result = _provider.Execute<IEnumerable<T>>(Expression);
        return result.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
