//
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Kusto.Language;
using Kusto.Language.Symbols;
using KustoLoco.Core.DataSource;
using KustoLoco.Core.DataSource.Columns;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

internal sealed class TakeAnyIfIntImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfint)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        MyDebug.Assert(valuesColumn.RowCount == predicatesColumn.RowCount);

        for (var i = 0; i < valuesColumn.RowCount; i++)
        {
            if (predicatesColumn[i] == true)
            {
                return new ScalarResult(ScalarTypes.Int, valuesColumn[i]);
            }
        }

        return new ScalarResult(ScalarTypes.Int, null);
    }
}

internal sealed class TakeAnyIfLongImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOflong)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        MyDebug.Assert(valuesColumn.RowCount == predicatesColumn.RowCount);

        for (var i = 0; i < valuesColumn.RowCount; i++)
        {
            if (predicatesColumn[i] == true)
            {
                return new ScalarResult(ScalarTypes.Long, valuesColumn[i]);
            }
        }

        return new ScalarResult(ScalarTypes.Long, null);
    }
}

internal sealed class TakeAnyIfDecimalImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfdecimal)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        MyDebug.Assert(valuesColumn.RowCount == predicatesColumn.RowCount);

        for (var i = 0; i < valuesColumn.RowCount; i++)
        {
            if (predicatesColumn[i] == true)
            {
                return new ScalarResult(ScalarTypes.Decimal, valuesColumn[i]);
            }
        }

        return new ScalarResult(ScalarTypes.Decimal, null);
    }
}

internal sealed class TakeAnyIfDoubleImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfdouble)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        MyDebug.Assert(valuesColumn.RowCount == predicatesColumn.RowCount);

        for (var i = 0; i < valuesColumn.RowCount; i++)
        {
            if (predicatesColumn[i] == true)
            {
                return new ScalarResult(ScalarTypes.Real, valuesColumn[i]);
            }
        }

        return new ScalarResult(ScalarTypes.Real, null);
    }
}

internal sealed class TakeAnyIfDateTimeImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfDateTime)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        MyDebug.Assert(valuesColumn.RowCount == predicatesColumn.RowCount);

        for (var i = 0; i < valuesColumn.RowCount; i++)
        {
            if (predicatesColumn[i] == true)
            {
                return new ScalarResult(ScalarTypes.DateTime, valuesColumn[i]);
            }
        }

        return new ScalarResult(ScalarTypes.DateTime, null);
    }
}

internal sealed class TakeAnyIfTimeSpanImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfTimeSpan)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        MyDebug.Assert(valuesColumn.RowCount == predicatesColumn.RowCount);

        for (var i = 0; i < valuesColumn.RowCount; i++)
        {
            if (predicatesColumn[i] == true)
            {
                return new ScalarResult(ScalarTypes.TimeSpan, valuesColumn[i]);
            }
        }

        return new ScalarResult(ScalarTypes.TimeSpan, null);
    }
}

internal sealed class TakeAnyIfGuidImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfGuid)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        MyDebug.Assert(valuesColumn.RowCount == predicatesColumn.RowCount);

        for (var i = 0; i < valuesColumn.RowCount; i++)
        {
            if (predicatesColumn[i] == true)
            {
                return new ScalarResult(ScalarTypes.Guid, valuesColumn[i]);
            }
        }

        return new ScalarResult(ScalarTypes.Guid, null);
    }
}

internal sealed class TakeAnyIfBoolImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfbool)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        MyDebug.Assert(valuesColumn.RowCount == predicatesColumn.RowCount);

        for (var i = 0; i < valuesColumn.RowCount; i++)
        {
            if (predicatesColumn[i] == true)
            {
                return new ScalarResult(ScalarTypes.Bool, valuesColumn[i]);
            }
        }

        return new ScalarResult(ScalarTypes.Bool, null);
    }
}

internal sealed class TakeAnyIfStringImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfstring)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        MyDebug.Assert(valuesColumn.RowCount == predicatesColumn.RowCount);

        for (var i = 0; i < valuesColumn.RowCount; i++)
        {
            if (predicatesColumn[i] == true)
            {
                return new ScalarResult(ScalarTypes.String, valuesColumn[i]);
            }
        }

        return new ScalarResult(ScalarTypes.String, null);
    }
}

internal sealed class TakeAnyIfJsonNodeImpl : IAggregateImpl
{
    public EvaluationResult Invoke(ITableChunk chunk, ColumnarResult[] arguments)
    {
        MyDebug.Assert(arguments.Length == 2);
        var valuesColumn = (GenericTypedBaseColumnOfJsonNode)arguments[0].Column;
        var predicatesColumn = (GenericTypedBaseColumnOfbool)arguments[1].Column;
        MyDebug.Assert(valuesColumn.RowCount == predicatesColumn.RowCount);

        for (var i = 0; i < valuesColumn.RowCount; i++)
        {
            if (predicatesColumn[i] == true)
            {
                return new ScalarResult(ScalarTypes.Dynamic, valuesColumn[i]);
            }
        }

        return new ScalarResult(ScalarTypes.Dynamic, null);
    }
}

internal static class TakeAnyIf
{
    internal static void Register(Dictionary<FunctionSymbol, AggregateInfo> aggregates)
    {
        var takeAnyIfOverloads = new AggregateInfo(
            new AggregateOverloadInfo(new TakeAnyIfIntImpl(), ScalarTypes.Int, ScalarTypes.Int, ScalarTypes.Bool),
            new AggregateOverloadInfo(new TakeAnyIfLongImpl(), ScalarTypes.Long, ScalarTypes.Long, ScalarTypes.Bool),
            new AggregateOverloadInfo(new TakeAnyIfDecimalImpl(), ScalarTypes.Decimal, ScalarTypes.Decimal, ScalarTypes.Bool),
            new AggregateOverloadInfo(new TakeAnyIfDoubleImpl(), ScalarTypes.Real, ScalarTypes.Real, ScalarTypes.Bool),
            new AggregateOverloadInfo(new TakeAnyIfDateTimeImpl(), ScalarTypes.DateTime, ScalarTypes.DateTime, ScalarTypes.Bool),
            new AggregateOverloadInfo(new TakeAnyIfTimeSpanImpl(), ScalarTypes.TimeSpan, ScalarTypes.TimeSpan, ScalarTypes.Bool),
            new AggregateOverloadInfo(new TakeAnyIfGuidImpl(), ScalarTypes.Guid, ScalarTypes.Guid, ScalarTypes.Bool),
            new AggregateOverloadInfo(new TakeAnyIfBoolImpl(), ScalarTypes.Bool, ScalarTypes.Bool, ScalarTypes.Bool),
            new AggregateOverloadInfo(new TakeAnyIfStringImpl(), ScalarTypes.String, ScalarTypes.String, ScalarTypes.Bool),
            new AggregateOverloadInfo(new TakeAnyIfJsonNodeImpl(), ScalarTypes.Dynamic, ScalarTypes.Dynamic, ScalarTypes.Bool)
        );
        aggregates.Add(Aggregates.TakeAnyIf, takeAnyIfOverloads);
    }
}
