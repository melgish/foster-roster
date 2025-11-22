using Radzen;
using Radzen.Blazor;

namespace FosterRoster.Shared.Components;

public class AppGridButton : RadzenButton
{
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        ButtonStyle = parameters.GetValueOrDefault(nameof(ButtonStyle), ButtonStyle.Primary);
        ButtonType = parameters.GetValueOrDefault(nameof(ButtonType), ButtonType.Button);
        Size = parameters.GetValueOrDefault(nameof(Size), ButtonSize.ExtraSmall);
        Variant = parameters.GetValueOrDefault(nameof(Variant), Variant.Outlined);

        await base.SetParametersAsync(parameters);
    }
}
