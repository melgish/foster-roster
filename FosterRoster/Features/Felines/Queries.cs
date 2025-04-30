namespace FosterRoster.Features.Felines;

public static class Queries
{
    /// <summary>
    ///     Map feline entity to grid row model.
    /// </summary>
    /// <param name="query">query instance to select from</param>
    /// <returns>IQueryable with mapping to edit model</returns>
    public static IQueryable<FelineGridDto> SelectToGridDto(this IQueryable<Feline> query)
        => query.Select(e => new FelineGridDto
        {
            AnimalId = e.AnimalId ?? string.Empty,
            FostererName = e.Fosterer != null ? e.Fosterer.Name : "",
            Id = e.Id,
            IsInactive = e.IsInactive,
            Name = e.Name
        });
}