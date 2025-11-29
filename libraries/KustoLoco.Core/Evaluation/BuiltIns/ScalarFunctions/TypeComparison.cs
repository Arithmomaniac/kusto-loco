//
// Licensed under the MIT License.

using System;
using System.Runtime.CompilerServices;
using KustoLoco.Core.Evaluation.BuiltIns.Impl;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

/// <summary>
///     This file was originally auto-generated but has been refactored
///     to use generic comparison helpers for types that implement IComparisonOperators.
///     DateTime and TimeSpan don't implement IComparisonOperators so they keep explicit implementations.
/// </summary>
public static class TypeComparison
{
    // Numeric types delegate to ComparisonHelper generic methods
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int? MaxOfInt(int? a, int? b) => ComparisonHelper.Max(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int? MinOfInt(int? a, int? b) => ComparisonHelper.Min(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long? MaxOfLong(long? a, long? b) => ComparisonHelper.Max(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long? MinOfLong(long? a, long? b) => ComparisonHelper.Min(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal? MaxOfDecimal(decimal? a, decimal? b) => ComparisonHelper.Max(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal? MinOfDecimal(decimal? a, decimal? b) => ComparisonHelper.Min(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double? MaxOfDouble(double? a, double? b) => ComparisonHelper.Max(a, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double? MinOfDouble(double? a, double? b) => ComparisonHelper.Min(a, b);

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
