using FosterRoster.Extensions;

namespace FosterRoster.Components.Pages.Dashboard;

public sealed record FelineCardDto
{
    public Category Category { get; init; }
    public Gender Gender { get; init; }
    public int Id { get; init; }
    public int? IntakeAgeInWeeks { get; init; }
    public DateOnly IntakeDate { get; init; }
    public string Name { get; init; } = string.Empty;
    public uint? ThumbnailVersion { get; set; }
    
    public string FormatAge(DateTimeOffset asOfDate)
        => FelineExtensions.FormatAge(IntakeAgeInWeeks, IntakeDate, asOfDate);
    
    public string GetThumbnailUrl()
        => ThumbnailExtensions.GetUrl(Id, ThumbnailVersion);
}