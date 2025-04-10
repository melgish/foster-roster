namespace FosterRoster.Features.Chores;

public sealed class CloneTemplateRequest
{
    /// <summary> Get/Sets the ID of the template to clone.</summary>
    public int ChoreId { get; set; }
    /// <summary>Gets the ID of the target feline to receive chore</summary>
    public int FelineId { get; init; }
}

[UsedImplicitly]
public sealed class CloneTemplateRequestValidator : AbstractValidator<CloneTemplateRequest>
{
    public CloneTemplateRequestValidator()
    {
        RuleFor(x => x.ChoreId)
            .NotEmpty()
            .WithName("Template");
        
        RuleFor(x => x.FelineId)
            .NotEmpty()
            .WithName("Feline");
    }
}