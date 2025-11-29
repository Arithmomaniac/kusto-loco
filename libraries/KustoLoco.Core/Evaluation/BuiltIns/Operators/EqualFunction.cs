using System;

// ReSharper disable PartialTypeWithSinglePart
namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

[KustoImplementation(Keyword = "Operators.Equal")]
internal partial class EqualFunction
{
    private static bool IntImpl(int a, int b) => ComparisonHelper.Equal(a, b);
    private static bool LongImpl(long a, long b) => ComparisonHelper.Equal(a, b);
    private static bool DoubleImpl(double a, double b) => ComparisonHelper.Equal(a, b);
    private static bool DecimalImpl(decimal a, decimal b) => ComparisonHelper.Equal(a, b);
    // TimeSpan and DateTime don't implement IEqualityOperators<T,T,bool>
    private static bool TsImpl(TimeSpan a, TimeSpan b) => a == b;
    private static bool DtImpl(DateTime a, DateTime b) => a == b;
    // String, bool, and Guid don't implement IEqualityOperators<T,T,bool>
    private static bool StrImpl(string a, string b) => a == b;
    private static bool BoolImpl(bool a, bool b) => a == b;
    private static bool GuilImpl(Guid a, Guid b) => a == b;
}
