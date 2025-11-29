using System;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

[KustoImplementation(Keyword = "Operators.GreaterThanOrEqual")]
internal partial class GreaterThanOrEqualFunction
{
    private static bool IntImpl(int a, int b) => ComparisonHelper.GreaterThanOrEqual(a, b);
    private static bool LongImpl(long a, long b) => ComparisonHelper.GreaterThanOrEqual(a, b);
    private static bool DoubleImpl(double a, double b) => ComparisonHelper.GreaterThanOrEqual(a, b);
    private static bool DecimalImpl(decimal a, decimal b) => ComparisonHelper.GreaterThanOrEqual(a, b);
    // TimeSpan and DateTime don't implement IComparisonOperators<T,T,bool>
    private static bool TsImpl(TimeSpan a, TimeSpan b) => a >= b;
    private static bool DtImpl(DateTime a, DateTime b) => a >= b;
}
