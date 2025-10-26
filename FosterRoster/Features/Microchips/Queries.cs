namespace FosterRoster.Features.Microchips;

public static class Queries
{
    public static IQueryable<MicrochipFormDto> SelectToFormDto(this IQueryable<Microchip> query)
        => query.Select(e => new MicrochipFormDto
        {
            Brand = e.Brand,
            Code = e.Code,
            Comment = e.Comment,
            FelineId = e.FelineId,
            Id = e.Id,
        });
}