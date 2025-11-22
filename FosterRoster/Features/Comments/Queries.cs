namespace FosterRoster.Features.Comments;

public static class Queries
{
    extension(Comment entity)
    {
        /// <summary>
        ///     Map supplied comment entity to edit model
        /// </summary>
        /// <returns>Edit model for the supplied entity</returns>
        public CommentFormDto ToFormDto()
            => new()
            {
                FelineId = entity.FelineId,
                Id = entity.Id,
                Modified = entity.Modified,
                Text = entity.Text,
                TimeStamp = entity.TimeStamp
            };
    }
}
