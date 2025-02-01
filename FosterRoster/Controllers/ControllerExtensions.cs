namespace FosterRoster.Controllers;

public static class ControllerExtensions
{
    public static UnprocessableEntityObjectResult Unprocessable<T>(this ControllerBase controller, Result<T> result)
        => controller.UnprocessableEntity(result.Errors.First().Message);

    public static UnprocessableEntityObjectResult Unprocessable(this ControllerBase controller, Result result)
        => controller.UnprocessableEntity(result.Errors.First().Message);
}