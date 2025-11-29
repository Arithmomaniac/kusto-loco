//
// Licensed under the MIT License.

using System;
using System.Text.Json.Nodes;
using KustoLoco.Core.DataSource;
using KustoLoco.Core.DataSource.Columns;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

internal class MakeListIfIntFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2 || arguments.Length == 3);
        var valuesColumn = (GenericTypedBaseColumnOfint)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 2);
        return MakeCollectionHelper.MakeListIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
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
        return MakeCollectionHelper.MakeListIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
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
        return MakeCollectionHelper.MakeListIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
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
        return MakeCollectionHelper.MakeListIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
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
        return MakeCollectionHelper.MakeListIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
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
        return MakeCollectionHelper.MakeListIf(valuesColumn.RowCount, i => valuesColumn[i], predicatesColumn, maxSize);
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
        return MakeCollectionHelper.MakeListIfString(valuesColumn, predicatesColumn, maxSize);
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
        return MakeCollectionHelper.MakeListIfDynamic(valuesColumn, predicatesColumn, maxSize);
    }
}
