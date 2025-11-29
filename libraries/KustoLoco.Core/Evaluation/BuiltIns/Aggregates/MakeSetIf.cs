//
// Licensed under the MIT License.

using System;
using KustoLoco.Core.DataSource;
using KustoLoco.Core.DataSource.Columns;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

internal class MakeSetIfIntFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2 || arguments.Length == 3);
        var valuesColumn = (GenericTypedBaseColumnOfint)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 2);
        return MakeCollectionHelper.MakeSetIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
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
        return MakeCollectionHelper.MakeSetIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
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
        return MakeCollectionHelper.MakeSetIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
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
        return MakeCollectionHelper.MakeSetIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
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
        return MakeCollectionHelper.MakeSetIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
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
        return MakeCollectionHelper.MakeSetIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
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
        return MakeCollectionHelper.MakeSetIfString(valuesColumn, predicatesColumn, maxSize);
    }
}
