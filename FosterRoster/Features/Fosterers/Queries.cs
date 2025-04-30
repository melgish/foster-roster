namespace FosterRoster.Features.Fosterers;

public static class Queries
{
    /// <summary>
    ///     Map fosterer entities to model for editing
    /// </summary>
    /// <param name="query">query instance to select from</param>
    /// <returns>IQueryable with mapping to edit model</returns>
    public static IQueryable<FostererFormDto> SelectToFormDto(this IQueryable<Fosterer> query)
        => query.Select(fosterer => new FostererFormDto
        {
            Address = fosterer.Address ?? string.Empty,
            ContactMethod = fosterer.ContactMethod,
            Email = fosterer.Email ?? string.Empty,
            Id = fosterer.Id,
            Name = fosterer.Name,
            Phone = fosterer.Phone ?? string.Empty
        });

    /// <summary>
    ///     Map fosterer entities to model for grid.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public static IQueryable<FostererGridDto> SelectToGridDto(this IQueryable<Fosterer> query)
        => query.Select(fosterer => new FostererGridDto
        {
            Email = fosterer.Email ?? string.Empty,
            Id = fosterer.Id,
            Name = fosterer.Name,
            Phone = fosterer.Phone ?? string.Empty
        });

    /// <summary>
    ///     Map fosterer entities to model for select list.
    /// </summary>
    /// <param name="query">query instance to select from</param>
    /// <returns>Updated query with mapping to list items.</returns>
    public static IQueryable<ListItemDto<int>> SelectToListItemDto(this IQueryable<Fosterer> query)
        => query.Select(fosterer => new ListItemDto<int>(fosterer.Id, fosterer.Name));

    /// <summary>
    ///     Transform a single fosterer entity into a dto for editing.
    /// </summary>
    /// <param name="fosterer"></param>
    /// <returns></returns>
    public static FostererFormDto ToFormDto(this Fosterer fosterer)
        => new()
        {
            Address = fosterer.Address ?? string.Empty,
            ContactMethod = fosterer.ContactMethod,
            Email = fosterer.Email ?? string.Empty,
            Id = fosterer.Id,
            Name = fosterer.Name,
            Phone = fosterer.Phone ?? string.Empty
        };
}