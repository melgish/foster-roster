namespace FosterRoster.Shared.Components;

using CronExpressionDescriptor;
using Microsoft.AspNetCore.Components.Rendering;

public sealed class CronDescription : ComponentBase
{
    private static readonly Options Options = new()
    {
        Verbose = true,
        ThrowExceptionOnParseError = false,
        Use24HourTimeFormat = false
    };

    [Parameter]
    public string? Cron { get; set; }

    private string? _description;

    protected override void OnParametersSet()
    {
        _description = string.IsNullOrWhiteSpace(Cron)
            ? string.Empty
            : ExpressionDescriptor.GetDescription(Cron, Options);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);  
        builder.AddContent(0, _description ?? string.Empty);
    }
}