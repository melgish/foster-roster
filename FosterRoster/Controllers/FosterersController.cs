namespace FosterRoster.Controllers;

[ApiController]
[Route("api/fosterers")]
public sealed class FosterersController(
    IFostererRepository fostererRepository,
    IValidator<FostererEditModel> fostererValidator
) : ControllerBase
{
    /// <summary>
    /// Adds a new cat to the database.
    /// </summary>
    /// <param name="model">Model containing fosterer data to add.</param>
    /// <returns>Updated fosterer instance after add.</returns>
    [HttpPost]
    public async Task<Fosterer> AddAsync(FostererEditModel model)
    {
        await fostererValidator.ValidateAndThrowAsync(model);
        return await fostererRepository.AddAsync(model.ToFosterer());
    }

    /// <summary>
    /// Get list of all cats in the database.
    /// </summary>
    /// <returns>List of cats, or empty list if no cats exist.</returns>
    [HttpGet]
    public async Task<List<Fosterer>> GetAllAsync()
        => await fostererRepository.GetAllAsync();

    /// <summary>
    /// Get list of all cats in the database.
    /// </summary>
    /// <returns>List of cats, or empty list if no cats exist.</returns>
    [HttpGet("names")]
    public async Task<List<ListItem<int>>> GetAllNamesAsync()
        => await fostererRepository.GetAllNamesAsync();

    /// <summary>
    /// Gets a single cat by ID.
    /// </summary>
    /// <param name="fostererId"></param>
    /// <returns>A single cat if found, otherwise null</returns>
    [HttpGet("{fostererId:int}")]
    public async Task<Fosterer?> GetByKeyAsync(int fostererId)
        => await fostererRepository.GetByKeyAsync(fostererId);
    
    /// <summary>
    /// Updates a Fosterer in the database.
    /// </summary>
    /// <param name="fostererId">ID of Fosterer to modify</param>
    /// <param name="model">Updated data to assign</param>
    /// <returns>Updated Fosterer if found, otherwise null</returns>
    [HttpPut("{fostererId:int}")]
    public async Task<Fosterer?> UpdateAsync(int fostererId, FostererEditModel model)
    {
        await fostererValidator.ValidateAndThrowAsync(model);
        return await fostererRepository.UpdateAsync(fostererId, model.ToFosterer());
    }
}