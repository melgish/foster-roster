namespace FosterRoster.Features.Chores;

using Microsoft.AspNetCore.Components.Forms;
using Radzen;

[UsedImplicitly]
public sealed partial class ConfirmTaskComplete(DialogService dialogService)
{
    [Parameter] public ChoreCompletionFormDto Model { get; set; } = null!;

    private void Cancel() => dialogService.Close(null);
    private void Confirm(EditContext context) => dialogService.Close(context.Model);
}

public static class ConfirmTaskCompleteExtensions
{
    /// <summary>
    /// Show the task completion confirmation dialog.
    /// </summary>
    /// <param name="dialogService"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public static async Task<ChoreCompletionFormDto?> ConfirmTaskCompleteAsync(
        this DialogService dialogService,
        ChoreCompletionFormDto input
    ) => (ChoreCompletionFormDto?) await dialogService.OpenAsync<ConfirmTaskComplete>(
        "Confirm Task Complete",
        new() { ["Model"] = input }
    );
}