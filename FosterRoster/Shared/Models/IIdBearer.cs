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
    extension(IIdBearer bearer)
    {
        public bool IsNew => bearer.Id == 0;
        
        public bool IsExisting => bearer.Id != 0;
        
        public IdOnlyDto ToIdOnly() => new(bearer.Id);
    }
}