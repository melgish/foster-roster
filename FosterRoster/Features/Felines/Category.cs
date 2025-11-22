namespace FosterRoster.Features.Felines;

using System.ComponentModel.DataAnnotations;

public enum Category
{
    Kitten = 1,
    Cat = 2,

    [Display(Description = "Nursing Kitten")]
    NursingKitten = 5,
    [Display(Description = "Nursing Cat")] NursingCat = 6
}
