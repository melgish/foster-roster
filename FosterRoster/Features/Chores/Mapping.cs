namespace FosterRoster.Features.Chores;

public static class Mapping
{
    /// <summary>
    ///     Map chore entity to grid row model.
    /// </summary>
    /// <param name="query">query instance to select from</param>
    /// <returns>IQueryable with mapping to edit model</returns>
    public static IQueryable<ChoreGridDto> SelectToGridDto(this IQueryable<Chore> query)
        => query.Select(e => new ChoreGridDto
        {
            DueDate = e.DueDate,
            FelineName = e.Feline == null ? "Template" : e.Feline.Name,
            Cron = e.Cron,
            Id = e.Id,
            Name = e.Name,
            Repeats = e.Repeats
        });
}