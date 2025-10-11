using FosterRoster.Features.Felines;

namespace FosterRoster.Features.Vaccinations;

public static class Queries
{
    public static IQueryable<VaccinationFormDto> SelectToFormDto(this IQueryable<Vaccination> query)
        => query.Select(e => new VaccinationFormDto
        {
            AdministeredBy = e.AdministeredBy,
            Comments = e.Comments,
            ExpirationDate = e.ExpirationDate,
            FelineId = e.FelineId,
            Id = e.Id,
            ManufacturerName = e.ManufacturerName,
            SerialNumber = e.SerialNumber,
            VaccinationDate = e.VaccinationDate,
            VaccineName = e.VaccineName,
        });
    
    public static IQueryable<VaccinationGridDto> SelectToGridDto(this IQueryable<Vaccination> query)
        => query.Select(e => new VaccinationGridDto()
        {
            ExpirationDate = e.ExpirationDate,
            FelineName = e.Feline.Name,
            Id = e.Id,
            VaccinationDate = e.VaccinationDate,
            VaccineName = e.VaccineName,
        });
    
    public static IQueryable<Vaccination> ForFeline(this IQueryable<Vaccination> query, int felineId)
        => query.Where(e => e.FelineId == felineId);
}