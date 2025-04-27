namespace FosterRoster.Shared.Interfaces;

public interface IConfirmDelete
{
    /// <summary>
    /// Name passed in to identify the thing being deleted.
    /// </summary>
    string Name { get; set; }
}