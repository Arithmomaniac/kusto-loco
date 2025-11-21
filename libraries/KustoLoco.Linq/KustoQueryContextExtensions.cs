// Licensed under the MIT License.

using System.Linq;
using KustoLoco.Core;

namespace KustoLoco.Linq;

/// <summary>
/// Extension methods for KustoQueryContext to enable LINQ queries.
/// </summary>
public static class KustoQueryContextExtensions
{
    /// <summary>
    /// Gets a queryable for a table in the Kusto context.
    /// </summary>
    /// <typeparam name="T">The type representing the table schema.</typeparam>
    /// <param name="context">The Kusto query context.</param>
    /// <param name="tableName">Optional table name. If not provided, uses the type name.</param>
    /// <returns>An IQueryable that can be used with LINQ.</returns>
    public static IQueryable<T> GetTable<T>(this KustoQueryContext context, string? tableName = null)
    {
        var provider = new KustoQueryProvider(context);
        var queryable = new KustoTableQueryable<T>(provider, tableName ?? typeof(T).Name);
        return queryable;
    }

    /// <summary>
    /// Gets a queryable for a table by name with dynamic row type.
    /// </summary>
    /// <param name="context">The Kusto query context.</param>
    /// <param name="tableName">The table name.</param>
    /// <returns>An IQueryable&lt;object[]&gt; that can be used with LINQ.</returns>
    public static IQueryable<object[]> GetTable(this KustoQueryContext context, string tableName)
    {
        var provider = new KustoQueryProvider(context);
        var queryable = new KustoTableQueryable<object[]>(provider, tableName);
        return queryable;
    }
}
