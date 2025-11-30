//
// Licensed under the MIT License.

using KustoLoco.Core.DataSource.Columns;

namespace KustoLoco.Core.Evaluation.BuiltIns.Impl;

/// <summary>
///     Helper class providing common functionality for MakeList, MakeSet, and related aggregate functions.
/// </summary>
internal static class MakeCollectionHelper
{
    /// <summary>
    ///     Extracts the optional max size parameter from the arguments.
    ///     Returns long.MaxValue if not specified or if the column is empty.
    /// </summary>
    public static long GetMaxSize(ColumnarResult[] arguments, int maxSizeIndex)
    {
        if (arguments.Length <= maxSizeIndex)
            return long.MaxValue;

        var maxSizeColumn = (GenericTypedBaseColumnOflong)arguments[maxSizeIndex].Column;
        return maxSizeColumn.RowCount > 0
            ? maxSizeColumn[0] ?? long.MaxValue
            : long.MaxValue;
    }
}
