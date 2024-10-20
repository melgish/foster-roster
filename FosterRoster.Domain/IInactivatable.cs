// spell-checker: ignore inactivatable
namespace FosterRoster.Domain;

public interface IInactivatable
{
    /// <summary>
    /// Gets or sets a value indicating whether the entity is inactive.
    /// </summary>
    bool IsInactive { get; set; }

    /// <summary>
    /// Gets or sets the date and time the entity was inactivated.
    /// </summary>
    DateTimeOffset? InactivatedAtUtc { get; set; }
}

