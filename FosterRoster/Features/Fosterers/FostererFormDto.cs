namespace FosterRoster.Features.Fosterers;

public sealed class FostererFormDto : IIdBearer
{
    /// <summary>
    ///     Mailing label style address of the Fosterer
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    ///     Gets / Sets the preferred contact method for the Fosterer
    /// </summary>
    public ContactMethod ContactMethod { get; set; } = ContactMethod.Email;

    /// <summary>
    ///     Email address of the Fosterer
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    ///     Unique identifier for the Fosterer
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     Gets / Sets teh name of the Fosterer
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Primary contact phone.
    /// </summary>
    public string? Phone { get; set; }
}