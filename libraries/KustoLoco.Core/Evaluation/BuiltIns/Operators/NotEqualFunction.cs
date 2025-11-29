using System;

// ReSharper disable PartialTypeWithSinglePart
namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

[KustoImplementation(Keyword = "Operators.NotEqual")]
internal partial class NotEqualFunction
{
    private static bool IntImpl(int a, int b) => ComparisonHelper.NotEqual(a, b);
    private static bool LongImpl(long a, long b) => ComparisonHelper.NotEqual(a, b);
    private static bool DoubleImpl(double a, double b) => ComparisonHelper.NotEqual(a, b);
    private static bool DecimalImpl(decimal a, decimal b) => ComparisonHelper.NotEqual(a, b);
    // TimeSpan and DateTime don't implement IEqualityOperators<T,T,bool>
    private static bool TsImpl(TimeSpan a, TimeSpan b) => a != b;
    private static bool DtImpl(DateTime a, DateTime b) => a != b;
    // String, bool, and Guid don't implement IEqualityOperators<T,T,bool>
    private static bool StrImpl(string a, string b) => a != b;
    private static bool BoolImpl(bool a, bool b) => a != b;
    private static bool GuidImpl(Guid a, Guid b) => a != b;
}
