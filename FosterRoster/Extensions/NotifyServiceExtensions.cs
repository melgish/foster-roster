namespace FosterRoster.Extensions;

using Radzen;

public static class NotifyServiceExtensions
{
    /// <summary>
    /// Notify user about the result of an action
    /// </summary>
    /// <param name="service">Notification Service to target</param>
    /// <param name="result">Result instance to check</param>
    /// <param name="successSummary">Summary for success message.</param>
    /// <param name="errorSummary">Summary for error message.</param>
    public static void Notify(
        this NotificationService service,
        IResultBase result,
        string successSummary = "Success",
        string errorSummary = "Error"
    ) => service.Notify(result switch
    {
        { IsSuccess: true } => new()
        {
            Detail = result.Successes.FirstOrDefault()?.Message,
            Severity = NotificationSeverity.Success,
            Summary = successSummary
        },
        { IsSuccess: false } => new()
        {
            Detail = result.Errors.FirstOrDefault()?.Message,
            Severity = NotificationSeverity.Error,
            Summary = errorSummary
        }
    });
}