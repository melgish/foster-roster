namespace FosterRoster.Features.Felines;

using Chores;
using Comments;
using Data;
using Fosterers;
using Sources;
using Thumbnails;
using Weights;

public sealed class Feline : IIdBearer
{
    public string? AnimalId { get; set; }
    public string? Breed { get; set; }
    public Category Category { get; set; }
    public ICollection<Chore> Chores { get; set; } = [];
    public string? Color { get; set; }
    public ICollection<Comment> Comments { get; set; } = [];
    public Fosterer? Fosterer { get; init; }
    public int? FostererId { get; set; }
    public Gender Gender { get; set; }
    public int Id { get; init; }
    public DateTimeOffset? InactivatedAtUtc { get; init; }
    public int? IntakeAgeInWeeks { get; set; }
    public DateOnly IntakeDate { get; set; }
    public bool IsInactive { get; init; }
    public string Name { get; set; } = string.Empty;
    public DateOnly? RegistrationDate { get; set; }
    public Source? Source { get; init; }
    public int? SourceId { get; set; }
    public Thumbnail? Thumbnail { get; set; }
    public Weaned Weaned { get; set; }
    public ICollection<Weight> Weights { get; init; } = [];
}