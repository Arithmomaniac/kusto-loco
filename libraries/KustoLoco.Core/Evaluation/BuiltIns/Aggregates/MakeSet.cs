//
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Kusto.Language.Symbols;
using KustoLoco.Core.DataSource;
using KustoLoco.Core.DataSource.Columns;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

internal static class MakeSetHelper
{
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
}

internal class MakeSetIntFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfint)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeSetHelper.MakeSet(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeSetLongFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOflong)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeSetHelper.MakeSet(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeSetDoubleFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfdouble)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeSetHelper.MakeSet(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeSetDecimalFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfdecimal)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeSetHelper.MakeSet(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeSetTimeSpanFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfTimeSpan)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeSetHelper.MakeSet(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeSetDateTimeFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfDateTime)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeSetHelper.MakeSet(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeSetStringFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfstring)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeSetHelper.MakeSetString(valuesColumn, maxSize);
    }
}
