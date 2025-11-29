//
// Licensed under the MIT License.

using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

/// <summary>
///     Helper class providing generic comparison operations using C# generic math interfaces.
/// </summary>
internal static class ComparisonHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equal<T>(T a, T b) where T : IEqualityOperators<T, T, bool>
        => a == b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotEqual<T>(T a, T b) where T : IEqualityOperators<T, T, bool>
        => a != b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThan<T>(T a, T b) where T : IComparisonOperators<T, T, bool>
        => a > b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThan<T>(T a, T b) where T : IComparisonOperators<T, T, bool>
        => a < b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThanOrEqual<T>(T a, T b) where T : IComparisonOperators<T, T, bool>
        => a >= b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanOrEqual<T>(T a, T b) where T : IComparisonOperators<T, T, bool>
        => a <= b;

    /// <summary>
    ///     Returns the maximum of two nullable values, returning a if b is null.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? Max<T>(T? a, T? b) where T : struct, IComparisonOperators<T, T, bool>
    {
        if (b is not { } bValue) return a;
        return a == null ? b : a.Value > bValue ? a : b;
    }

    /// <summary>
    ///     Returns the minimum of two nullable values, returning a if b is null.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? Min<T>(T? a, T? b) where T : struct, IComparisonOperators<T, T, bool>
    {
        if (b is not { } bValue) return a;
        return a == null ? b : a.Value < bValue ? a : b;
    }
}
