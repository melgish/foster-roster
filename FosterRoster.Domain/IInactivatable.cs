// spell-checker: ignore inactivatable
namespace FosterRoster.Domain;

public interface IInactivatable
{
    bool IsInactive { get; set; }

    DateTimeOffset? InactivatedAtUtc { get; set; }
}

