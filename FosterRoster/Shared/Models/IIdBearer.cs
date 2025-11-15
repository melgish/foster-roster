namespace FosterRoster.Shared.Models;

public interface IIdBearer
{
    /// <summary>
    /// Gets the ID of the entity.
    /// </summary>
    int Id { get; }
}

public sealed record IdOnlyDto(int Id) : IIdBearer;

public static class IdOnly
{
    /// <summary>
    ///     Result when a single ID cannot be resolved from multiple updates.
    /// </summary>
    public static readonly IdOnlyDto Zero = new(0);
    
    extension(IIdBearer bearer)
    {
        /// <summary>
        ///     Test if entity is new.
        /// </summary>
        public bool IsNew => bearer.Id == 0;
        
        /// <summary>
        ///     Test if entity has been persisted to the database.
        /// </summary>
        public bool IsExisting => bearer.Id != 0;
        
        /// <summary>
        ///     Convert entity to an IdOnlyDto.
        /// </summary>
        /// <returns></returns>
        public IdOnlyDto ToIdOnly() => new(bearer.Id);
    }
}