namespace FosterRoster.Features.Vaccinations;

public sealed class VaccinationGridDto
{
    public DateOnly? ExpirationDate { get; init; }

    public string FelineName { get; init; } = string.Empty;

    public int Id { get; init; }

    public DateOnly VaccinationDate { get; init; }

    public string VaccineName { get; init; } = string.Empty;
}