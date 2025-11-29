//
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Kusto.Language.Symbols;
using KustoLoco.Core.DataSource;
using KustoLoco.Core.DataSource.Columns;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

/// <summary>
///     Helper class providing common functionality for MakeList, MakeSet, and related aggregate functions.
/// </summary>
internal static class MakeCollectionHelper
{
    /// <summary>
    ///     Extracts the optional max size parameter from the arguments.
    ///     Returns long.MaxValue if not specified or if the column is empty.
    /// </summary>
    public static long GetMaxSize(ColumnarResult[] arguments, int maxSizeIndex)
    {
        if (arguments.Length <= maxSizeIndex)
            return long.MaxValue;

        var maxSizeColumn = (GenericTypedBaseColumnOflong)arguments[maxSizeIndex].Column;
        return maxSizeColumn.RowCount > 0
            ? maxSizeColumn[0] ?? long.MaxValue
            : long.MaxValue;
    }

    /// <summary>
    ///     Creates a list from a value column using the provided accessors.
    /// </summary>
    public static EvaluationResult MakeList<T>(
        int rowCount,
        Func<int, T?> getValue,
        long maxSize)
        where T : struct
    {
        var list = new List<T>();
        for (var i = 0; i < rowCount; i++)
        {
            var v = getValue(i);
            if (v.HasValue)
            {
                list.Add(v.Value);
                if (list.Count >= maxSize)
                    break;
            }
        }

        return new ScalarResult(ScalarTypes.Dynamic, JsonArrayHelper.From(list));
    }

    /// <summary>
    ///     Creates a list from a column, filtered by a predicate column, using the provided accessors.
    /// </summary>
    public static EvaluationResult MakeListIf<T>(
        int rowCount,
        Func<int, T?> getValue,
        GenericTypedBaseColumnOfbool predicatesColumn,
        long maxSize)
        where T : struct
    {
        MyDebug.Assert(rowCount == predicatesColumn.RowCount);

        var list = new List<T>();
        for (var i = 0; i < rowCount; i++)
        {
            if (predicatesColumn[i] == true)
            {
                var v = getValue(i);
                if (v.HasValue)
                {
                    list.Add(v.Value);
                    if (list.Count >= maxSize)
                        break;
                }
            }
        }

        return new ScalarResult(ScalarTypes.Dynamic, JsonArrayHelper.From(list));
    }

    /// <summary>
    ///     Creates a list including null values using the provided accessor.
    /// </summary>
    public static EvaluationResult MakeListWithNulls<T>(
        int rowCount,
        Func<int, T?> getValue)
        where T : struct
    {
        var list = new List<T?>();
        for (var i = 0; i < rowCount; i++)
        {
            list.Add(getValue(i));
        }

        return new ScalarResult(ScalarTypes.Dynamic, JsonArrayHelper.From(list));
    }

    /// <summary>
    ///     Creates a set (unique values) using the provided accessor.
    /// </summary>
    public static EvaluationResult MakeSet<T>(
        int rowCount,
        Func<int, T?> getValue,
        long maxSize)
        where T : struct
    {
        var set = new HashSet<T>();
        for (var i = 0; i < rowCount; i++)
        {
            var v = getValue(i);
            if (v.HasValue)
            {
                set.Add(v.Value);
                if (set.Count >= maxSize)
                    break;
            }
        }

        return new ScalarResult(ScalarTypes.Dynamic, JsonArrayHelper.From(set));
    }

    /// <summary>
    ///     Creates a set (unique values), filtered by a predicate column, using the provided accessor.
    /// </summary>
    public static EvaluationResult MakeSetIf<T>(
        int rowCount,
        Func<int, T?> getValue,
        GenericTypedBaseColumnOfbool predicatesColumn,
        long maxSize)
        where T : struct
    {
        MyDebug.Assert(rowCount == predicatesColumn.RowCount);

        var set = new HashSet<T>();
        for (var i = 0; i < rowCount; i++)
        {
            if (predicatesColumn[i] == true)
            {
                var v = getValue(i);
                if (v.HasValue)
                {
                    set.Add(v.Value);
                    if (set.Count >= maxSize)
                        break;
                }
            }
        }

        return new ScalarResult(ScalarTypes.Dynamic, JsonArrayHelper.From(set));
    }

    // String-specific overloads (strings are reference types with different null handling)

    /// <summary>
    ///     Creates a list from a string column, skipping null or empty values.
    /// </summary>
    public static EvaluationResult MakeListString(GenericTypedBaseColumnOfstring valuesColumn, long maxSize)
    {
        var list = new List<string>();
        for (var i = 0; i < valuesColumn.RowCount; i++)
        {
            var v = valuesColumn[i];
            if (!string.IsNullOrEmpty(v))
            {
                list.Add(v);
                if (list.Count >= maxSize)
                    break;
            }
        }

        return new ScalarResult(ScalarTypes.Dynamic, JsonArrayHelper.From(list));
    }

    /// <summary>
    ///     Creates a list from a string column, filtered by a predicate column, skipping null or empty values.
    /// </summary>
    public static EvaluationResult MakeListIfString(
        GenericTypedBaseColumnOfstring valuesColumn,
        GenericTypedBaseColumnOfbool predicatesColumn,
        long maxSize)
    {
        MyDebug.Assert(valuesColumn.RowCount == predicatesColumn.RowCount);

        var list = new List<string>();
        for (var i = 0; i < valuesColumn.RowCount; i++)
        {
            if (predicatesColumn[i] == true)
            {
                var v = valuesColumn[i];
                if (!string.IsNullOrEmpty(v))
                {
                    list.Add(v);
                    if (list.Count >= maxSize)
                        break;
                }
            }
        }

        return new ScalarResult(ScalarTypes.Dynamic, JsonArrayHelper.From(list));
    }

    /// <summary>
    ///     Creates a list including null values from a string column.
    /// </summary>
    public static EvaluationResult MakeListWithNullsString(GenericTypedBaseColumnOfstring valuesColumn)
    {
        var list = new List<string?>();
        for (var i = 0; i < valuesColumn.RowCount; i++)
        {
            list.Add(valuesColumn[i]);
        }

        return new ScalarResult(ScalarTypes.Dynamic, JsonArrayHelper.From(list));
    }

    /// <summary>
    ///     Creates a set (unique values) from a string column, skipping null or empty values.
    /// </summary>
    public static EvaluationResult MakeSetString(GenericTypedBaseColumnOfstring valuesColumn, long maxSize)
    {
        var set = new HashSet<string>();
        for (var i = 0; i < valuesColumn.RowCount; i++)
        {
            var v = valuesColumn[i];
            if (!string.IsNullOrEmpty(v))
            {
                set.Add(v);
                if (set.Count >= maxSize)
                    break;
            }
        }

        return new ScalarResult(ScalarTypes.Dynamic, JsonArrayHelper.From(set));
    }

    /// <summary>
    ///     Creates a set (unique values) from a string column, filtered by a predicate column, skipping null or empty values.
    /// </summary>
    public static EvaluationResult MakeSetIfString(
        GenericTypedBaseColumnOfstring valuesColumn,
        GenericTypedBaseColumnOfbool predicatesColumn,
        long maxSize)
    {
        MyDebug.Assert(valuesColumn.RowCount == predicatesColumn.RowCount);

        var set = new HashSet<string>();
        for (var i = 0; i < valuesColumn.RowCount; i++)
        {
            if (predicatesColumn[i] == true)
            {
                var v = valuesColumn[i];
                if (!string.IsNullOrEmpty(v))
                {
                    set.Add(v);
                    if (set.Count >= maxSize)
                        break;
                }
            }
        }

        return new ScalarResult(ScalarTypes.Dynamic, JsonArrayHelper.From(set));
    }

    // Dynamic (JsonNode) specific overloads

    /// <summary>
    ///     Creates a list from a dynamic (JsonNode) column, skipping null values.
    /// </summary>
    public static EvaluationResult MakeListDynamic(GenericTypedBaseColumnOfJsonNode valuesColumn, long maxSize)
    {
        var list = new List<JsonNode?>();
        for (var i = 0; i < valuesColumn.RowCount; i++)
        {
            var v = valuesColumn[i];
            if (v == null) continue;
            list.Add(v);
            if (list.Count >= maxSize)
                break;
        }

        return new ScalarResult(ScalarTypes.Dynamic, JsonArrayHelper.From(list));
    }

    /// <summary>
    ///     Creates a list from a dynamic (JsonNode) column, filtered by a predicate column, skipping null values.
    /// </summary>
    public static EvaluationResult MakeListIfDynamic(
        GenericTypedBaseColumnOfJsonNode valuesColumn,
        GenericTypedBaseColumnOfbool predicatesColumn,
        long maxSize)
    {
        MyDebug.Assert(valuesColumn.RowCount == predicatesColumn.RowCount);

        var list = new List<JsonNode?>();
        for (var i = 0; i < valuesColumn.RowCount; i++)
        {
            if (predicatesColumn[i] != true) continue;

            var v = valuesColumn[i];
            if (v == null) continue;
            list.Add(v);
            if (list.Count >= maxSize)
                break;
        }

        return new ScalarResult(ScalarTypes.Dynamic, JsonArrayHelper.From(list));
    }
}
