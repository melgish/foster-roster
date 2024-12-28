namespace FosterRoster.Domain.Validation;

public static class ValidatorExtensions
{
    public static async Task<Result<TModel>> ValidateAndMapToResultAsync<TModel>(this IValidator<TModel> validator,
        TModel model)
    {
        var validationResult = await validator.ValidateAsync(model);
        if (validationResult.IsValid) return Result.Ok(model);
        return Result
            .Fail("The model is not valid")
            .WithErrors(validationResult.Errors.Select(error => error.ErrorMessage));
    }
}