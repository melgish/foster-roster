using FosterRoster.Features.Felines;

namespace FosterRoster.Features.Dashboard;

public static class Queries
{
    extension(IQueryable<Feline> query)
    {
        public IQueryable<FelineCardDto> SelectToCardDto()
            => query.Select(f => new FelineCardDto
            {
                Category = f.Category,
                Gender = f.Gender,
                Id = f.Id,
                IntakeAgeInWeeks = f.IntakeAgeInWeeks,
                IntakeDate = f.IntakeDate,
                Name = f.Name,
                ThumbnailVersion = f.Thumbnail == null ? null : f.Thumbnail.Version
            });
    }
}
