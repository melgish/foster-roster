using FosterRoster.Features.Felines;
using System.Security.Claims;

namespace FosterRoster.Features.Dashboard;

public static class Queries
{
    public static IQueryable<FelineCardDto> SelectToCardDto(this IQueryable<Feline> query)
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