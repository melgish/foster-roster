namespace FosterRoster.Components.Shared;

using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

public class AppFormButton : RadzenButton
{
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        ButtonStyle = parameters.GetValueOrDefault(nameof(ButtonStyle), ButtonStyle.Primary);
        ButtonType = parameters.GetValueOrDefault(nameof(ButtonType), ButtonType.Button);
        Size = parameters.GetValueOrDefault(nameof(Size), ButtonSize.Small);
        Variant = parameters.GetValueOrDefault(nameof(Variant), Variant.Outlined);

        await base.SetParametersAsync(parameters);
    }
}