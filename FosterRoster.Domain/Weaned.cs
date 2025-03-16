namespace FosterRoster.Domain;

using System.ComponentModel.DataAnnotations;

public enum Weaned
{
    /// <summary>
    /// Feline has not been weaned.
    /// </summary>
    No = 1,

    /// <summary>
    /// Feline is being weaned.
    /// </summary>
    [Display(Description = "In Progress")] InProgress = 2,

    /// <summary>
    /// Feline has been weaned.
    /// </summary>
    Yes = 3
}