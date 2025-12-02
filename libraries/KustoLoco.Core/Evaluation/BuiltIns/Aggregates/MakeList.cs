//
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Kusto.Language.Symbols;
using KustoLoco.Core.DataSource;
using KustoLoco.Core.DataSource.Columns;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

internal static class MakeListHelper
{
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
}

internal class MakeListIntFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfint)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeListHelper.MakeList(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeListLongFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOflong)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeListHelper.MakeList(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeListDoubleFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfdouble)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeListHelper.MakeList(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeListDecimalFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfdecimal)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeListHelper.MakeList(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeListTimeSpanFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfTimeSpan)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeListHelper.MakeList(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeListDateTimeFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfDateTime)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeListHelper.MakeList(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeListStringFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfstring)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeListHelper.MakeListString(valuesColumn, maxSize);
    }
}

internal class MakeListDynamicFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfJsonNode)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeListHelper.MakeListDynamic(valuesColumn, maxSize);
    }
}
