namespace FosterRoster.Shared.Models;

public interface IConfirmDelete
{
    /// <summary>
    /// Name passed in to identify the thing being deleted.
    /// </summary>
    [UsedImplicitly]
    string Name { get; set; }
}
