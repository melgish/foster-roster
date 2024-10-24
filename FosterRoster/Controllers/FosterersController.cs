namespace FosterRoster.Controllers;

[ApiController]
[Route("api/fosterers")]
public sealed class FosterersController(
    IFostererRepository fostererRepository
) : ControllerBase
{

}