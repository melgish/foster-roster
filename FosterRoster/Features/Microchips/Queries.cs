namespace FosterRoster.Features.Microchips;

public static class Queries
{
    extension(IQueryable<Microchip> query)
    {
        public IQueryable<MicrochipFormDto> SelectToFormDto()
            => query.Select(e => new MicrochipFormDto
            {
                Brand = e.Brand,
                Code = e.Code,
                Comment = e.Comment,
                FelineId = e.FelineId,
                Id = e.Id,
            });
    }
}