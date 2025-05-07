namespace FosterRoster.Features.Chores;

using Shared.Models;

public static class Queries
{
    /// <summary>
    ///     Limit results to a single Feline.
    /// </summary>
    /// <param name="query">query instance to select from</param>
    /// <param name="felineId">ID of feline to filter on.</param>
    /// <returns>Updated query that filters results on a specific feline.</returns>
    public static IQueryable<Chore> ForFeline(this IQueryable<Chore> query, int felineId)
        => query.Where(e => e.FelineId == felineId);

    /// <summary>
    ///     Limit results to templates
    /// </summary>
    /// <param name="query">query instance to select from</param>
    /// <returns>Updated query that only returns templates.</returns>
    public static IQueryable<Chore> OnlyTemplates(this IQueryable<Chore> query)
        => query.Where(e => !e.FelineId.HasValue);

    /// <summary>
    ///     Returns assigned chores that are due before the specified date.
    /// </summary>
    /// <param name="query">query instance to select from</param>
    /// <param name="dueDate">Due date to query after</param>
    /// <returns></returns>
    public static IQueryable<Chore> DueBefore(this IQueryable<Chore> query, DateTimeOffset dueDate)
        => query
            .Where(e => e.FelineId.HasValue)
            .Where(e => !e.DueDate.HasValue || e.DueDate <= dueDate);
    
    
    /// <summary>
    ///     Map chore entity to edit form model.
    /// </summary>
    /// <param name="query">query instance to select from</param>
    /// <returns>Updated query that maps results for editing</returns>
    public static IQueryable<ChoreFormDto> SelectToFormDto(this IQueryable<Chore> query)
        => query.Select(chore => new ChoreFormDto
        {
            Cron = chore.Cron,
            Description = chore.Description,
            DueDate = chore.DueDate,
            FelineId = chore.FelineId.GetValueOrDefault(),
            Id = chore.Id,
            Name = chore.Name,
            Repeats = chore.Repeats
        });

    /// <summary>
    ///     Map chore entity to grid row model.
    /// </summary>
    /// <param name="query">query instance to select from</param>
    /// <returns>Updated query that maps results for grid rows</returns>
    public static IQueryable<ChoreGridDto> SelectToGridDto(this IQueryable<Chore> query)
        => query.Select(e => new ChoreGridDto
        {
            Cron = e.Cron,
            DueDate = e.DueDate,
            FelineName = e.Feline == null ? "Template" : e.Feline.Name,
            Id = e.Id,
            Name = e.Name,
            Repeats = e.Repeats
        });

    /// <summary>
    ///     Map task entity for display in a select list.
    /// </summary>
    /// <param name="query">query instance to select from</param>
    /// <returns>Updated query that returns list items</returns>
    public static IQueryable<ListItemDto<int>> SelectToListItemDto(this IQueryable<Chore> query)
        => query.Select(e => new ListItemDto<int>(e.Id, e.Name));
}