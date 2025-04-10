namespace FosterRoster.Shared.Components;

using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

/// <summary>
///     Base for wrapped components that are used in forms.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class AppFormComponent<TValue> : ComponentBase
{
    [Parameter] public bool Disabled { get; set; }

    protected string Name { get; } = Guid.NewGuid().ToString();

    [Parameter] public string? Placeholder { get; set; }

    [Parameter] public string Text { get; set; } = null!;

    [Parameter] public TValue Value { get; set; } = default!;

    [Parameter] public EventCallback<TValue> ValueChanged { get; set; }

    [Parameter] public Expression<Func<TValue>> ValueExpression { get; set; } = null!;
}