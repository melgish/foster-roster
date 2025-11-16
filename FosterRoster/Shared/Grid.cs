namespace FosterRoster.Shared;

using Radzen;

/// <summary>
///     A set of configurable defaults for Radzen Grids used within the application.
/// </summary>
public static class Grid
{
    public static readonly Density Density = Density.Compact;
    public static readonly FilterCaseSensitivity FilterCaseSensitivity = FilterCaseSensitivity.CaseInsensitive;
    public const int PageNumbersCount = 1;
    public const HorizontalAlign PagerHorizontalAlign = HorizontalAlign.Center;
    public static readonly PagerPosition PagerPosition = PagerPosition.Top;
    public const int PageSize = 20;
    public static readonly int[] PageSizeOptions = [10, 20, 50, 100];
    public const string PagingSummaryFormat = "Showing {0} to {1} of {2}";
}