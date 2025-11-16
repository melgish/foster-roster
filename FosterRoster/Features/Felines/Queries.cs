using FosterRoster.Features.Comments;
using FosterRoster.Features.Thumbnails;

namespace FosterRoster.Features.Felines;

public static class Queries
{
    /// <param name="query">query instance to select from</param>
    extension(IQueryable<Feline> query)
    {
        /// <summary>
        ///     Map feline entity to grid row model.
        /// </summary>
        /// <returns>IQueryable with mapping to edit model</returns>
        public IQueryable<FelineGridDto> SelectToGridDto()
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
        /// <returns></returns>
        public IQueryable<FelineFormDto> SelectToFormDto()
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
                SourceId = entity.SourceId ?? 0,
                SterilizationDate = entity.SterilizationDate,
                Thumbnail = entity.Thumbnail == null
                    ? null
                    : new Thumbnail
                    {
                        FelineId = entity.Thumbnail.FelineId,
                        ContentType = entity.Thumbnail.ContentType,
                        Version = entity.Thumbnail.Version
                    },
                Weaned = entity.Weaned
            }).AsSplitQuery();
    }
}