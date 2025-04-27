namespace FosterRoster.Features.Fosterers;

public static class Mapping
{
    /// <summary>
    ///     Map source entities to edit model
    /// </summary>
    /// <param name="query">query instance to select from</param>
    /// <returns>IQueryable with mapping to edit model</returns>
    public static IQueryable<FostererFormDto> SelectToFormDto(this IQueryable<Fosterer> query)
        => query.Select(e => new FostererFormDto
        {
            Id = e.Id,
            Name = e.Name
        });
    
}