//
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Kusto.Language.Symbols;
using KustoLoco.Core.DataSource;
using KustoLoco.Core.DataSource.Columns;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

internal static class MakeSetIfHelper
{
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
}

internal class MakeSetIfIntFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2 || arguments.Length == 3);
        var valuesColumn = (GenericTypedBaseColumnOfint)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 2);
        return MakeSetIfHelper.MakeSetIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
    }
}

internal class MakeSetIfLongFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2 || arguments.Length == 3);
        var valuesColumn = (GenericTypedBaseColumnOflong)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 2);
        return MakeSetIfHelper.MakeSetIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
    }
}

internal class MakeSetIfDoubleFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2 || arguments.Length == 3);
        var valuesColumn = (GenericTypedBaseColumnOfdouble)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 2);
        return MakeSetIfHelper.MakeSetIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
    }
}

internal class MakeSetIfDecimalFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2 || arguments.Length == 3);
        var valuesColumn = (GenericTypedBaseColumnOfdecimal)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 2);
        return MakeSetIfHelper.MakeSetIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
    }
}

internal class MakeSetIfTimeSpanFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2 || arguments.Length == 3);
        var valuesColumn = (GenericTypedBaseColumnOfTimeSpan)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 2);
        return MakeSetIfHelper.MakeSetIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
    }
}

internal class MakeSetIfDateTimeFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2 || arguments.Length == 3);
        var valuesColumn = (GenericTypedBaseColumnOfDateTime)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 2);
        return MakeSetIfHelper.MakeSetIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
    }
}

internal class MakeSetIfStringFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2 || arguments.Length == 3);
        var valuesColumn = (GenericTypedBaseColumnOfstring)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 2);
        return MakeSetIfHelper.MakeSetIfString(valuesColumn, predicatesColumn, maxSize);
    }
}
