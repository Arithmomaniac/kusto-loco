// Licensed under the MIT License.

using System.Collections;
using System.Linq.Expressions;

namespace KustoLoco.Linq;

/// <summary>
/// Represents a Kusto query that can be used with LINQ.
/// Implements IQueryable&lt;T&gt; to enable LINQ query composition.
/// </summary>
/// <typeparam name="T">The type of elements in the query.</typeparam>
public class KustoQueryable<T> : IOrderedQueryable<T>
{
    private readonly KustoQueryProvider _provider;
    private readonly Expression _expression;

    internal KustoQueryable(KustoQueryProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _expression = Expression.Constant(this);
    }

    internal KustoQueryable(KustoQueryProvider provider, Expression expression)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _expression = expression ?? throw new ArgumentNullException(nameof(expression));

        if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
        {
            throw new ArgumentException("Expression must be of type IQueryable<T>", nameof(expression));
        }
    }

    public Type ElementType => typeof(T);

    public Expression Expression => _expression;

    public IQueryProvider Provider => _provider;

    public IEnumerator<T> GetEnumerator()
    {
        var result = _provider.Execute<IEnumerable<T>>(_expression);
        return result.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Executes the query asynchronously and returns the result.
    /// </summary>
    public async Task<KustoQueryResult> ExecuteAsync()
    {
        return await _provider.ExecuteAsync(_expression);
    }

    /// <summary>
    /// Gets the KQL query string that would be executed for this LINQ query.
    /// </summary>
    public string ToKql()
    {
        var translator = new LinqToKqlTranslator();
        return translator.Translate(_expression);
    }
}
