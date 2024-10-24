namespace FosterRoster.Domain.Repositories;

public interface ISourceRepository
{
    public Task<List<Source>> GetAllAsync();

    public Task<List<ListItem<int>>> GetAllNamesAsync();
}
