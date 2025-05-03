namespace FosterRoster.Features.Weights;

internal static class Queries
{
    /// <summary>
    ///     Limit requested weights to those for a specific feline
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="felineId"></param>
    /// <returns></returns>
    public static IQueryable<Weight> ForFeline(this IQueryable<Weight> queryable, int felineId)
        => felineId == 0 ? queryable : queryable.Where(w => w.FelineId == felineId);

    /// <summary>
    ///     Transform Weight entities for grid
    /// </summary>
    /// <param name="query">query instance to select from</param>
    /// <returns></returns>
    public static IQueryable<WeightGridDto> SelectToGridDto(this IQueryable<Weight> query)
        => query.Select(weight => new WeightGridDto
        {
            FelineId = weight.FelineId,
            DateTime = weight.DateTime,
            Name = weight.Feline.Name,
            Units = weight.Units,
            Value = weight.Value
        });
}