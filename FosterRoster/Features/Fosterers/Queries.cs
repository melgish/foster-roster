namespace FosterRoster.Features.Fosterers;

public static class Queries
{
    extension(IQueryable<Fosterer> query)
    {
        /// <summary>
        ///     Map fosterer entities to model for editing
        /// </summary>
        /// <returns>IQueryable with mapping to edit model</returns>
        public IQueryable<FostererFormDto> SelectToFormDto()
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
        /// <returns></returns>
        public IQueryable<FostererGridDto> SelectToGridDto()
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
        /// <returns>Updated query with mapping to list items.</returns>
        public IQueryable<ListItemDto<int>> SelectToListItemDto()
            => query.Select(fosterer => new ListItemDto<int>(fosterer.Id, fosterer.Name));
    }
}