using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace FosterRoster.Features.Chores;

[UsedImplicitly]
public sealed partial class ConfirmTaskComplete(DialogService dialogService)
{
    [Parameter] public ChoreCompletionFormDto Model { get; set; } = null!;

    private void Cancel() => dialogService.Close();
    private void Confirm(EditContext context) => dialogService.Close(context.Model);


    /// <summary>
    ///     Prompts the user to confirm the completion of a chore and logs the completion if confirmed.
    /// </summary>
    /// <param name="choreGridDto"></param>
    /// <param name="choreRepository"></param>
    /// <param name="dialogService"></param>
    /// <param name="notificationService"></param>
    /// <param name="timeProvider"></param>
    /// <returns></returns>
    public static async Task<bool> ConfirmAndCompleteAsync(
        ChoreGridDto choreGridDto,
        ChoreRepository choreRepository,
        DialogService dialogService,
        NotificationService notificationService,
        TimeProvider timeProvider
    )
    {
        ChoreCompletionFormDto? logEntry = (ChoreCompletionFormDto?)await dialogService.OpenAsync<ConfirmTaskComplete>(
        "Confirm Task Complete",
        new Dictionary<string, object>
        {
            ["Model"] = new ChoreCompletionFormDto
            {
                LogDate = timeProvider.GetLocalNow(), LogText = choreGridDto.Description
            }
        });
        if (logEntry == null)
        {
            return false;
        }

        Result rs = await choreRepository.LogChoreCompletedAsync(choreGridDto.Id, logEntry);
        return notificationService.NotifyResult(rs, "Task", "log", "logged");
    }
}
