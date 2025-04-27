namespace FosterRoster.Features.Weights;

public static class Mapping
{
    /// <summary>
    /// Transform Weight entities for grid
    /// </summary>
    /// <param name="query">query instance to select from</param>
    /// <returns></returns>
    public static IQueryable<WeightGridDto> SelectToGridDto(this IQueryable<Weight> query)
    {
        return query.Select(weight => new WeightGridDto
        {
            FelineId = weight.FelineId,
            DateTime = weight.DateTime,
            Name = weight.Feline.Name,
            Units = weight.Units,
            Value = weight.Value
        });
    }
}