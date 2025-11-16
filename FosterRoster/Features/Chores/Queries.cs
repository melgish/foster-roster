namespace FosterRoster.Features.Chores;

public static class Queries
{
    extension(IQueryable<Chore> query)
    {
        /// <summary>
        ///     Limit results to a single Feline.
        /// </summary>
        /// <param name="felineId">ID of feline to filter on.</param>
        /// <returns>Updated query that filters results on a specific feline.</returns>
        public IQueryable<Chore> ForFeline(int felineId)
            => query.Where(e => e.FelineId == felineId);

        /// <summary>
        ///     Map chore entity to edit form model.
        /// </summary>
        /// <returns>Updated query that maps results for editing</returns>
        public IQueryable<ChoreFormDto> SelectToFormDto()
            => query.Select(chore => new ChoreFormDto
            {
                Description = chore.Description,
                DueDate = chore.DueDate,
                FelineId = chore.FelineId.GetValueOrDefault(),
                Id = chore.Id,
                Name = chore.Name,
            });

        /// <summary>
        ///     Map chore entity to grid row model.
        /// </summary>
        /// <returns>Updated query that maps results for grid rows</returns>
        public IQueryable<ChoreGridDto> SelectToGridDto()
            => query.Select(e => new ChoreGridDto
            {
                Description = e.Description ?? string.Empty,
                DueDate = e.DueDate,
                FelineName = e.Feline == null ? "Template" : e.Feline.Name,
                Id = e.Id,
                Name = e.Name,
            });
    }
}