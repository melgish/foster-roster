namespace FosterRoster.Domain.Repositories;

public interface IFostererRepository
{
    public Task<List<Fosterer>> GetAllAsync();

    public Task<List<ListItem<int>>> GetAllNamesAsync();
}
