namespace FosterRoster.Features.Schedules;

public static class Mapping
{
    /// <summary>
    ///     Map entities to edit model
    /// </summary>
    /// <param name="query">query to select from</param>
    /// <returns>IQueryable with mapping to edit model</returns>
    public static IQueryable<ScheduleFormDto> SelectToFormDto(this IQueryable<Schedule> query)
        => query.Select(e => new ScheduleFormDto
        {
            Cron = e.Cron,
            Id = e.Id,
            Name = e.Name
        });

    /// <summary>
    ///     Map supplied entity for the grid
    /// </summary>
    /// <param name="query">query to select from</param>
    /// <returns>IQueryable with mapping to grid row model</returns>
    public static IQueryable<ScheduleGridDto> SelectToGridDto(this IQueryable<Schedule> query)
        => query.Select(e => new ScheduleGridDto
        {
            Cron = e.Cron,
            Id = e.Id,
            Name = e.Name
        });

    /// <summary>
    ///     Map supplied entity to edit model
    /// </summary>
    /// <param name="entity">Entity to transform</param>
    /// <returns>Edit model for the supplied entity</returns>
    public static ScheduleFormDto ToFromDto(this Schedule entity)
        => new()
        {
            Cron = entity.Cron,
            Id = entity.Id,
            Name = entity.Name
        };
}