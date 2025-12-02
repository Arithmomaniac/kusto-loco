//
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Kusto.Language.Symbols;
using KustoLoco.Core.DataSource;
using KustoLoco.Core.DataSource.Columns;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

internal static class MakeListWithNullsHelper
{
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

    public static EvaluationResult MakeListWithNullsString(GenericTypedBaseColumnOfstring valuesColumn)
    {
        var list = new List<string?>();
        for (var i = 0; i < valuesColumn.RowCount; i++)
        {
            list.Add(valuesColumn[i]);
        }

        return new ScalarResult(ScalarTypes.Dynamic, JsonArrayHelper.From(list));
    }
}

internal class MakeListWithNullsIntFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1);
        var valuesColumn = (GenericTypedBaseColumnOfint)arguments[0].Column;
        return MakeListWithNullsHelper.MakeListWithNulls(valuesColumn.RowCount, i => valuesColumn[i]);
    }
}

internal class MakeListWithNullsLongFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1);
        var valuesColumn = (GenericTypedBaseColumnOflong)arguments[0].Column;
        return MakeListWithNullsHelper.MakeListWithNulls(valuesColumn.RowCount, i => valuesColumn[i]);
    }
}

internal class MakeListWithNullsDoubleFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1);
        var valuesColumn = (GenericTypedBaseColumnOfdouble)arguments[0].Column;
        return MakeListWithNullsHelper.MakeListWithNulls(valuesColumn.RowCount, i => valuesColumn[i]);
    }
}

internal class MakeListWithNullsDecimalFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1);
        var valuesColumn = (GenericTypedBaseColumnOfdecimal)arguments[0].Column;
        return MakeListWithNullsHelper.MakeListWithNulls(valuesColumn.RowCount, i => valuesColumn[i]);
    }
}

internal class MakeListWithNullsTimeSpanFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1);
        var valuesColumn = (GenericTypedBaseColumnOfTimeSpan)arguments[0].Column;
        return MakeListWithNullsHelper.MakeListWithNulls(valuesColumn.RowCount, i => valuesColumn[i]);
    }
}

internal class MakeListWithNullsDateTimeFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1);
        var valuesColumn = (GenericTypedBaseColumnOfDateTime)arguments[0].Column;
        return MakeListWithNullsHelper.MakeListWithNulls(valuesColumn.RowCount, i => valuesColumn[i]);
    }
}

internal class MakeListWithNullsStringFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1);
        var valuesColumn = (GenericTypedBaseColumnOfstring)arguments[0].Column;
        return MakeListWithNullsHelper.MakeListWithNullsString(valuesColumn);
    }
}
