﻿namespace FosterRoster.Features.Felines;

using Comments;

public static class Queries
{
    /// <summary>
    ///     Map feline entity to grid row model.
    /// </summary>
    /// <param name="query">query instance to select from</param>
    /// <returns>IQueryable with mapping to edit model</returns>
    public static IQueryable<FelineGridDto> SelectToGridDto(this IQueryable<Feline> query)
        => query.Select(entity => new FelineGridDto
        {
            AnimalId = entity.AnimalId ?? string.Empty,
            FostererName = entity.Fosterer != null ? entity.Fosterer.Name : "",
            Id = entity.Id,
            IsInactive = entity.IsInactive,
            Name = entity.Name
        });

    /// <summary>
    ///     Map feline result to edit model.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public static IQueryable<FelineFormDto> SelectToFormDto(this IQueryable<Feline> query)
        => query.Select(entity => new FelineFormDto
        {
            AnimalId = entity.AnimalId ?? string.Empty,
            Breed = entity.Breed ?? string.Empty,
            Category = entity.Category,
            Color = entity.Color ?? string.Empty,
            Comments = entity.Comments
                .OrderByDescending(c => c.TimeStamp)
                .Select(c => c.ToFormDto())
                .ToList(),
            FostererId = entity.FostererId ?? 0,
            Gender = entity.Gender,
            Id = entity.Id,
            IntakeAgeInWeeks = entity.IntakeAgeInWeeks,
            IsInactive = entity.IsInactive,
            InactivatedAtUtc = entity.InactivatedAtUtc,
            IntakeDate = entity.IntakeDate,
            Name = entity.Name,
            RegistrationDate = entity.RegistrationDate,
            SourceId = entity.SourceId ?? 0,
            Thumbnail = entity.Thumbnail == null
                ? null
                : new()
                {
                    FelineId = entity.Thumbnail.FelineId,
                    ContentType = entity.Thumbnail.ContentType,
                    Version = entity.Thumbnail.Version
                },
            Weaned = entity.Weaned
        }).AsSplitQuery();
}