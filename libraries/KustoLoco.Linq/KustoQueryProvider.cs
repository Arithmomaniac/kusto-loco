// Licensed under the MIT License.

using System.Linq.Expressions;

namespace KustoLoco.Linq;

/// <summary>
/// Query provider for Kusto LINQ queries.
/// Translates LINQ expressions to KQL and executes them against a KustoQueryContext.
/// </summary>
public class KustoQueryProvider : IQueryProvider
{
    private readonly KustoQueryContext _context;

    internal KustoQueryProvider(KustoQueryContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IQueryable CreateQuery(Expression expression)
    {
        var elementType = expression.Type.GetGenericArguments()[0];
        try
        {
            return (IQueryable)Activator.CreateInstance(
                typeof(KustoQueryable<>).MakeGenericType(elementType),
                new object[] { this, expression })!;
        }
        catch (System.Reflection.TargetInvocationException tie)
        {
            throw tie.InnerException ?? tie;
        }
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new KustoQueryable<TElement>(this, expression);
    }

    public object? Execute(Expression expression)
    {
        var translator = new LinqToKqlTranslator();
        var kql = translator.Translate(expression);
        
        // Execute the query synchronously (blocking)
        var result = _context.RunQuery(kql).GetAwaiter().GetResult();
        
        if (!string.IsNullOrEmpty(result.Error))
        {
            throw new InvalidOperationException($"Query execution failed: {result.Error}");
        }

        return result;
    }

    public TResult Execute<TResult>(Expression expression)
    {
        var translator = new LinqToKqlTranslator();
        var kql = translator.Translate(expression);
        
        // Execute the query synchronously (blocking)
        var result = _context.RunQuery(kql).GetAwaiter().GetResult();
        
        if (!string.IsNullOrEmpty(result.Error))
        {
            throw new InvalidOperationException($"Query execution failed: {result.Error}");
        }

        // Convert result to the expected type
        return ConvertResult<TResult>(result);
    }

    internal async Task<KustoQueryResult> ExecuteAsync(Expression expression)
    {
        var translator = new LinqToKqlTranslator();
        var kql = translator.Translate(expression);
        
        var result = await _context.RunQuery(kql);
        
        if (!string.IsNullOrEmpty(result.Error))
        {
            throw new InvalidOperationException($"Query execution failed: {result.Error}");
        }

        return result;
    }

    private TResult ConvertResult<TResult>(KustoQueryResult result)
    {
        var resultType = typeof(TResult);

        // Handle scalar results (Count, Sum, Average, etc.)
        if (resultType.IsPrimitive || resultType == typeof(string) || 
            resultType == typeof(decimal) || resultType == typeof(DateTime) ||
            Nullable.GetUnderlyingType(resultType) != null)
        {
            if (result.RowCount == 0)
            {
                return default!;
            }
            var value = result.GetRow(0)[0];
            if (value == null)
            {
                return default!;
            }
            return (TResult)Convert.ChangeType(value, Nullable.GetUnderlyingType(resultType) ?? resultType);
        }

        // Handle enumerable results
        if (resultType.IsGenericType)
        {
            var genericTypeDef = resultType.GetGenericTypeDefinition();
            if (genericTypeDef == typeof(IEnumerable<>) || genericTypeDef == typeof(List<>))
            {
                var elementType = resultType.GetGenericArguments()[0];
                var listType = typeof(List<>).MakeGenericType(elementType);
                var list = (System.Collections.IList)Activator.CreateInstance(listType)!;

                for (int i = 0; i < result.RowCount; i++)
                {
                    var row = result.GetRow(i);
                    var item = ConvertRowToObject(row, elementType, result);
                    list.Add(item);
                }

                return (TResult)list;
            }
        }

        // Return the raw result if we can't convert it
        return (TResult)(object)result;
    }

    private object ConvertRowToObject(object?[] row, Type targetType, KustoQueryResult result)
    {
        // If the target type is object[], just return the row
        if (targetType == typeof(object[]))
        {
            return row;
        }

        // If there's only one column and the target type is a primitive/simple type
        if (row.Length == 1)
        {
            var value = row[0];
            if (value == null)
            {
                return targetType.IsValueType ? Activator.CreateInstance(targetType)! : null!;
            }
            
            if (targetType.IsAssignableFrom(value.GetType()))
            {
                return value;
            }
            
            return Convert.ChangeType(value, targetType);
        }

        // For complex types, create an instance and populate properties
        var instance = Activator.CreateInstance(targetType)!;
        var properties = targetType.GetProperties();

        for (int i = 0; i < Math.Min(row.Length, properties.Length); i++)
        {
            var prop = properties[i];
            if (prop.CanWrite && row[i] != null)
            {
                var value = row[i];
                if (prop.PropertyType.IsAssignableFrom(value!.GetType()))
                {
                    prop.SetValue(instance, value);
                }
                else
                {
                    prop.SetValue(instance, Convert.ChangeType(value, prop.PropertyType));
                }
            }
        }

        return instance;
    }
}
