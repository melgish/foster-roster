namespace FosterRoster.Domain;

public class Fosterer : IInactivatable
{
    /// <summary>
    /// Mailing label style address of the Fosterer
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets / Sets the preferred contact method for the Fosterer
    /// </summary>
    public ContactMethod ContactMethod { get; set; } = ContactMethod.Email;

    /// <summary>
    /// Email address of the Fosterer
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Felines fostered by this Fosterer
    /// </summary>
    public virtual ICollection<Feline> Felines { get; set; } = [];

    /// <summary>
    /// Unique identifier for the Fosterer
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Date and time the Fosterer was inactivated
    /// </summary>
    public DateTimeOffset? InactivatedAtUtc { get; set; }

    /// <summary>
    /// True if the Fosterer is not active.
    /// </summary>
    public bool IsInactive { get; set; }

    /// <summary>
    /// Gets / Sets teh name of the Fosterer
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Primary contact phone.
    /// </summary>
    public string? Phone { get; set; }
}