namespace FosterRoster.Features.Sources;

public static class Queries
{
    extension(IQueryable<Source> query)
    {
        /// <summary>
        ///     Map source entities to edit model
        /// </summary>
        /// <returns>IQueryable with mapping to edit model</returns>
        public IQueryable<SourceFormDto> SelectToFormDto()
            => query.Select(e => new SourceFormDto
            {
                Id = e.Id,
                Name = e.Name
            });

        /// <summary>
        ///     Map source entities to grid row model
        /// </summary>
        /// <returns>IQueryable with mapping to grid row model</returns>
        public IQueryable<SourceGridDto> SelectToGridDto()
            => query.Select(e => new SourceGridDto
            {
                Id = e.Id,
                Name = e.Name
            });

        /// <summary>
        ///     Map supplied query results to list item
        /// </summary>
        /// <returns></returns>
        public IQueryable<ListItemDto<int>> SelectToListItemDto()
            => query.Select(e => new ListItemDto<int>(e.Id, e.Name));
    }
}