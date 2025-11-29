using System;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

[KustoImplementation(Keyword = "Operators.LessThanOrEqual")]
internal partial class LessThanOrEqualFunction
{
    private static bool IntImpl(int a, int b) => ComparisonHelper.LessThanOrEqual(a, b);
    private static bool LongImpl(long a, long b) => ComparisonHelper.LessThanOrEqual(a, b);
    private static bool DoubleImpl(double a, double b) => ComparisonHelper.LessThanOrEqual(a, b);
    private static bool DecimalImpl(decimal a, decimal b) => ComparisonHelper.LessThanOrEqual(a, b);
    // TimeSpan and DateTime don't implement IComparisonOperators<T,T,bool>
    private static bool TsImpl(TimeSpan a, TimeSpan b) => a <= b;
    private static bool DtImpl(DateTime a, DateTime b) => a <= b;
}
