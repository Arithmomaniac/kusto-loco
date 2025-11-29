//
// Licensed under the MIT License.

using System;
using KustoLoco.Core.DataSource;
using KustoLoco.Core.DataSource.Columns;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

internal class MakeListWithNullsIntFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1);
        var valuesColumn = (GenericTypedBaseColumnOfint)arguments[0].Column;
        return MakeCollectionHelper.MakeListWithNulls(valuesColumn.RowCount, i => valuesColumn[i]);
    }
}

internal class MakeListWithNullsLongFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1);
        var valuesColumn = (GenericTypedBaseColumnOflong)arguments[0].Column;
        return MakeCollectionHelper.MakeListWithNulls(valuesColumn.RowCount, i => valuesColumn[i]);
    }
}

internal class MakeListWithNullsDoubleFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1);
        var valuesColumn = (GenericTypedBaseColumnOfdouble)arguments[0].Column;
        return MakeCollectionHelper.MakeListWithNulls(valuesColumn.RowCount, i => valuesColumn[i]);
    }
}

internal class MakeListWithNullsDecimalFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1);
        var valuesColumn = (GenericTypedBaseColumnOfdecimal)arguments[0].Column;
        return MakeCollectionHelper.MakeListWithNulls(valuesColumn.RowCount, i => valuesColumn[i]);
    }
}

internal class MakeListWithNullsTimeSpanFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1);
        var valuesColumn = (GenericTypedBaseColumnOfTimeSpan)arguments[0].Column;
        return MakeCollectionHelper.MakeListWithNulls(valuesColumn.RowCount, i => valuesColumn[i]);
    }
}

internal class MakeListWithNullsDateTimeFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1);
        var valuesColumn = (GenericTypedBaseColumnOfDateTime)arguments[0].Column;
        return MakeCollectionHelper.MakeListWithNulls(valuesColumn.RowCount, i => valuesColumn[i]);
    }
}

internal class MakeListWithNullsStringFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1);
        var valuesColumn = (GenericTypedBaseColumnOfstring)arguments[0].Column;
        return MakeCollectionHelper.MakeListWithNullsString(valuesColumn);
    }
}
