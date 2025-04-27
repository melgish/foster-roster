namespace FosterRoster.Features.Comments;

public static class Mapping
{
    /// <summary>
    ///     Map supplied comment entity to edit model
    /// </summary>
    /// <param name="entity">Entity to transform</param>
    /// <returns>Edit model for the supplied entity</returns>
    public static CommentFormDto ToFormDto(this Comment entity)
        => new()
        {
            FelineId = entity.FelineId,
            Id = entity.Id,
            Text = entity.Text
        };
    
}