//
// Licensed under the MIT License.

using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

/// <summary>
///     This file was originally auto-generated but has been refactored
///     to use generic comparison helpers for types that implement IComparisonOperators.
///     DateTime and TimeSpan don't implement IComparisonOperators so they keep explicit implementations.
/// </summary>
public static class TypeComparison
{
    /// <summary>
    ///     Returns the maximum of two nullable values for types that implement IComparisonOperators.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? Max<T>(T? a, T? b) where T : struct, IComparisonOperators<T, T, bool>
    {
        if (b is not { } bValue) return a;
        return a == null ? b : a.Value > bValue ? a : b;
    }

    /// <summary>
    ///     Returns the minimum of two nullable values for types that implement IComparisonOperators.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? Min<T>(T? a, T? b) where T : struct, IComparisonOperators<T, T, bool>
    {
        if (b is not { } bValue) return a;
        return a == null ? b : a.Value < bValue ? a : b;
    }

    // Numeric types use the generic helpers
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int? MaxOfInt(int? a, int? b) => Max(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int? MinOfInt(int? a, int? b) => Min(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long? MaxOfLong(long? a, long? b) => Max(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long? MinOfLong(long? a, long? b) => Min(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal? MaxOfDecimal(decimal? a, decimal? b) => Max(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal? MinOfDecimal(decimal? a, decimal? b) => Min(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double? MaxOfDouble(double? a, double? b) => Max(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double? MinOfDouble(double? a, double? b) => Min(a, b);

    // DateTime and TimeSpan don't implement IComparisonOperators, so they keep explicit implementations
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime? MaxOfDateTime(DateTime? a, DateTime? b)
    {
        if (b is not { } bValue) return a;
        return a == null ? b : a.Value > bValue ? a : b;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime? MinOfDateTime(DateTime? a, DateTime? b)
    {
        if (b is not { } bValue) return a;
        return a == null ? b : a.Value < bValue ? a : b;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan? MaxOfTimeSpan(TimeSpan? a, TimeSpan? b)
    {
        if (b is not { } bValue) return a;
        return a == null ? b : a.Value > bValue ? a : b;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan? MinOfTimeSpan(TimeSpan? a, TimeSpan? b)
    {
        if (b is not { } bValue) return a;
        return a == null ? b : a.Value < bValue ? a : b;
    }
}
