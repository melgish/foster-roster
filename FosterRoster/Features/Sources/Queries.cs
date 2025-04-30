namespace FosterRoster.Features.Sources;

using Radzen.Blazor.Markdown;

public static class Queries
{
    /// <summary>
    ///     Map source entities to edit model
    /// </summary>
    /// <param name="query">query to select from</param>
    /// <returns>IQueryable with mapping to edit model</returns>
    public static IQueryable<SourceFormDto> SelectToFormDto(this IQueryable<Source> query)
        => query.Select(e => new SourceFormDto
        {
            Id = e.Id,
            Name = e.Name
        });

    /// <summary>
    ///     Map source entities to grid row model
    /// </summary>
    /// <param name="query">query to select from</param>
    /// <returns>IQueryable with mapping to grid row model</returns>
    public static IQueryable<SourceGridDto> SelectToGridDto(this IQueryable<Source> query)
        => query.Select(e => new SourceGridDto
        {
            Id = e.Id,
            Name = e.Name
        });

    /// <summary>
    ///     Map supplied query results to list item
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public static IQueryable<ListItemDto<int>> SelectToListItemDto(this IQueryable<Source> query)
        => query.Select(e => new ListItemDto<int>(e.Id, e.Name));

    /// <summary>
    ///     Map supplied source entity to edit model
    /// </summary>
    /// <param name="entity">Entity to transform</param>
    /// <returns>Edit model for the supplied entity</returns>
    public static SourceFormDto ToFormDto(this Source entity)
        => new()
        {
            Id = entity.Id,
            Name = entity.Name
        };
}