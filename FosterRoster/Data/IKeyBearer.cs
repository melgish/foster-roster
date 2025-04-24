namespace FosterRoster.Data;

public interface IKeyBearer
{
    /// <summary>
    /// Gets the ID of the entity.
    /// </summary>
    public int Id { get; }
}