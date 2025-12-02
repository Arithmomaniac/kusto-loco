//
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Kusto.Language.Symbols;
using KustoLoco.Core.DataSource;
using KustoLoco.Core.DataSource.Columns;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

internal static class MakeListIfHelper
{
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

internal class MakeListIfIntFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2 || arguments.Length == 3);
        var valuesColumn = (GenericTypedBaseColumnOfint)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 2);
        return MakeListIfHelper.MakeListIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
    }
}

internal class MakeListIfLongFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2 || arguments.Length == 3);
        var valuesColumn = (GenericTypedBaseColumnOflong)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 2);
        return MakeListIfHelper.MakeListIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
    }
}

internal class MakeListIfDoubleFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2 || arguments.Length == 3);
        var valuesColumn = (GenericTypedBaseColumnOfdouble)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 2);
        return MakeListIfHelper.MakeListIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
    }
}

internal class MakeListIfDecimalFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2 || arguments.Length == 3);
        var valuesColumn = (GenericTypedBaseColumnOfdecimal)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 2);
        return MakeListIfHelper.MakeListIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
    }
}

internal class MakeListIfTimeSpanFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2 || arguments.Length == 3);
        var valuesColumn = (GenericTypedBaseColumnOfTimeSpan)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 2);
        return MakeListIfHelper.MakeListIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
    }
}

internal class MakeListIfDateTimeFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2 || arguments.Length == 3);
        var valuesColumn = (GenericTypedBaseColumnOfDateTime)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 2);
        return MakeListIfHelper.MakeListIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
    }
}

internal class MakeListIfStringFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2 || arguments.Length == 3);
        var valuesColumn = (GenericTypedBaseColumnOfstring)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 2);
        return MakeListIfHelper.MakeListIfString(valuesColumn, predicatesColumn, maxSize);
    }
}

internal class MakeListIfDynamicFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2 || arguments.Length == 3);
        var valuesColumn = (GenericTypedBaseColumnOfJsonNode)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 2);
        return MakeListIfHelper.MakeListIfDynamic(valuesColumn, predicatesColumn, maxSize);
    }
}
