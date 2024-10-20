namespace FosterRoster.Domain;

public interface ISourceRepository
{
    public Task<List<Source>> GetAllAsync();

    public Task<List<ListItem<int>>> GetAllNamesAsync();
}
