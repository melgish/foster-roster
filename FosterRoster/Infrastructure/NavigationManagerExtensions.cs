namespace FosterRoster.Infrastructure;

public static class NavigationManagerExtensions
{
    public static bool VerifyFound(
        this NavigationManager navigationManager,
        IResultBase result)
    {
        if (result.IsSuccess)
            return true;

        navigationManager.NavigateTo("/not-found");
        return false;
    }
}
