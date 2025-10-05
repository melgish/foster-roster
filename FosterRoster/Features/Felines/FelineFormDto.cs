namespace FosterRoster.Features.Felines;

using Comments;
using Thumbnails;

public sealed class FelineFormDto : IIdBearer
{
    public string? AnimalId { get; set; }
    public string? Breed { get; set; }
    public Category Category { get; set; }
    public string? Color { get; set; } = string.Empty;
    public ICollection<CommentFormDto> Comments { get; set; } = [];
    public int FostererId { get; set; }
    public Gender Gender { get; set; }
    public int Id { get; init; }
    public int? IntakeAgeInWeeks { get; set; }
    public bool IsInactive { get; init; }
    public DateTimeOffset? InactivatedAtUtc { get; init; }
    public DateOnly? IntakeDate { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly? SterilizationDate { get; set; }
    public int SourceId { get; set; }
    public Thumbnail? Thumbnail { get; set; }
    public Weaned Weaned { get; set; }
}