using System;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

[KustoImplementation(Keyword = "Operators.LessThan")]
internal partial class LessThanFunction
{
    private static bool IntImpl(int a, int b) => ComparisonHelper.LessThan(a, b);
    private static bool LongImpl(long a, long b) => ComparisonHelper.LessThan(a, b);
    private static bool DoubleImpl(double a, double b) => ComparisonHelper.LessThan(a, b);
    private static bool DecimalImpl(decimal a, decimal b) => ComparisonHelper.LessThan(a, b);
    // TimeSpan and DateTime don't implement IComparisonOperators<T,T,bool>
    private static bool TsImpl(TimeSpan a, TimeSpan b) => a < b;
    private static bool DtImpl(DateTime a, DateTime b) => a < b;
}
