namespace FosterRoster.Domain;

using System.ComponentModel.DataAnnotations;

public enum WeightUnit
{
    // Names are intentionally lowercase to match the database values.
    // Resharper disable InconsistentNaming

    [Display(Description = "Grams", Name = "Grams", ShortName = "g")]
    g = 0,

    [Display(Description = "Ounces", Name = "Ounces", ShortName = "oz")]
    oz = 1,

    [Display(Description = "Pounds", Name = "Pounds", ShortName = "lbs")]
    lbs = 2,

    [Display(Description = "Kilograms", Name = "Kilograms", ShortName = "kg")]
    kg = 3

    // Resharper restore InconsistentNaming
}