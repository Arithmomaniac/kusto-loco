//
// Licensed under the MIT License.

using System;
using KustoLoco.Core.DataSource;
using KustoLoco.Core.DataSource.Columns;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

internal class MakeSetIntFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfint)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeCollectionHelper.MakeSet(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeSetLongFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOflong)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeCollectionHelper.MakeSet(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeSetDoubleFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfdouble)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeCollectionHelper.MakeSet(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeSetDecimalFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfdecimal)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeCollectionHelper.MakeSet(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeSetTimeSpanFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfTimeSpan)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeCollectionHelper.MakeSet(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeSetDateTimeFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfDateTime)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeCollectionHelper.MakeSet(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeSetStringFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfstring)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeCollectionHelper.MakeSetString(valuesColumn, maxSize);
    }
}
