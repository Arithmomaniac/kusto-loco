//
// Licensed under the MIT License.

using System;
using System.Text.Json.Nodes;
using KustoLoco.Core.DataSource;
using KustoLoco.Core.DataSource.Columns;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

internal class MakeListIntFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfint)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeCollectionHelper.MakeList(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeListLongFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOflong)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeCollectionHelper.MakeList(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeListDoubleFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfdouble)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeCollectionHelper.MakeList(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeListDecimalFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfdecimal)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeCollectionHelper.MakeList(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeListTimeSpanFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfTimeSpan)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeCollectionHelper.MakeList(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeListDateTimeFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfDateTime)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeCollectionHelper.MakeList(valuesColumn.RowCount, i => valuesColumn[i], maxSize);
    }
}

internal class MakeListStringFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfstring)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeCollectionHelper.MakeListString(valuesColumn, maxSize);
    }
}

internal class MakeListDynamicFunctionImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 1 || arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfJsonNode)arguments[0].Column;
        var maxSize = MakeCollectionHelper.GetMaxSize(arguments, 1);
        return MakeCollectionHelper.MakeListDynamic(valuesColumn, maxSize);
    }
}
