namespace FosterRoster.Features.Weights;

internal static class Queries
{
    extension(IQueryable<Weight> queryable)
    {
        /// <summary>
        ///     Limit requested weights to those for a specific feline
        /// </summary>
        /// <param name="felineId"></param>
        /// <returns></returns>
        public IQueryable<Weight> ForFeline(int felineId)
            => felineId == 0 ? queryable : queryable.Where(w => w.FelineId == felineId);

        /// <summary>
        ///     Transform Weight entities for grid
        /// </summary>
        /// <returns></returns>
        public IQueryable<WeightGridDto> SelectToGridDto()
            => queryable.Select(weight => new WeightGridDto
            {
                FelineId = weight.FelineId,
                DateTime = weight.DateTime,
                Name = weight.Feline.Name,
                Units = weight.Units,
                Value = weight.Value
            });
    }
}
