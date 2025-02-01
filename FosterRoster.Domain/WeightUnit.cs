namespace FosterRoster.Domain;

using System.ComponentModel.DataAnnotations;

public enum WeightUnit
{
    [Display(Description = "Grams", Name = "Grams")]
    g = 0,

    [Display(Description = "Ounces", Name = "Ounces")]
    oz = 1,

    [Display(Description = "Pounds", Name = "Pounds")]
    lbs = 2,

    [Display(Description = "Kilograms", Name = "Kilograms")]
    kg = 3
}