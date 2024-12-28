namespace FosterRoster.Domain;

public sealed class FostererEditModel()
{
    public FostererEditModel(Fosterer fosterer) : this()
    {
        Address = fosterer.Address;
        ContactMethod = fosterer.ContactMethod;
        Email = fosterer.Email;
        Id = fosterer.Id;
        InactivatedAtUtc = fosterer.InactivatedAtUtc;
        IsInactive = fosterer.IsInactive;
        Name = fosterer.Name;
        Phone = fosterer.Phone;
    }

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
    public int Id { get; set; }

    /// <summary>
    ///     Date and time the Fosterer was inactivated
    /// </summary>
    public DateTimeOffset? InactivatedAtUtc { get; set; }

    /// <summary>
    ///     True if the Fosterer is not active.
    /// </summary>
    public bool IsInactive { get; set; }

    /// <summary>
    ///     Gets / Sets teh name of the Fosterer
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Primary contact phone.
    /// </summary>
    public string? Phone { get; set; }

    public Fosterer ToFosterer() =>
        new()
        {
            Address = string.IsNullOrWhiteSpace(Address) ? null : Address.Trim(),
            ContactMethod = ContactMethod,
            Email = string.IsNullOrWhiteSpace(Email) ? null : Email.Trim(),
            Id = Id,
            InactivatedAtUtc = InactivatedAtUtc,
            IsInactive = IsInactive,
            Name = Name.Trim(),
            Phone = string.IsNullOrWhiteSpace(Phone) ? null : Phone.Trim()
        };
}