namespace FosterRoster.Features.Vaccinations;

public static class Queries
{
    extension(IQueryable<Vaccination> query)
    {
        public IQueryable<VaccinationFormDto> SelectToFormDto()
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
                VaccineName = e.VaccineName
            });

        public IQueryable<VaccinationGridDto> SelectToGridDto()
            => query.Select(e => new VaccinationGridDto
            {
                ExpirationDate = e.ExpirationDate,
                FelineName = e.Feline.Name,
                Id = e.Id,
                VaccinationDate = e.VaccinationDate,
                VaccineName = e.VaccineName
            });

        public IQueryable<Vaccination> ForFeline(int felineId)
            => query.Where(e => e.FelineId == felineId);
    }
}
