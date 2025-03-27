namespace FosterRoster.Components.Pages.Weights;

internal static class Queries
{
    public static IQueryable<Weight> ForFeline(this IQueryable<Weight> queryable, int felineId)
        => felineId == 0 ? queryable : queryable.Where(w => w.FelineId == felineId);
 
}