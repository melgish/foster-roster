namespace FosterRoster.Domain.Repositories;

public interface IChoresRepository
{
    /// <summary>
    ///     Adds a new chore to the database.
    /// </summary>
    /// <param name="chore">Chore instance to add.</param>
    /// <returns>A Result with Chore on Success, otherwise Result with Errors.</returns>
    public Task<Result<Chore>> AddAsync(Chore chore);
    
    /// <summary>
    ///     Deletes a Chore by its ID.
    /// </summary>
    /// <param name="choreId">ID of chore to remove.</param>
    /// <returns>A Result instance indicating success or failure.</returns>
    public Task<Result> DeleteByKeyAsync(int choreId);

}