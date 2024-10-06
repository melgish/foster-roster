using FosterRoster.Domain;

using Microsoft.EntityFrameworkCore;

namespace FosterRoster.Data;

public static class FosterRosterDbContextSeedData
{
    public static async Task SeedAsync(this FosterRosterDbContext context)
    {
        if (await context.Felines.AnyAsync())
        {
            return;
        }

        context.Felines.AddRange(
            new Feline()
            {
                Name = "Pipin",
                Gender = Gender.Male,
                IntakeDate = new DateOnly(2024, 7, 3),
                Category = Category.Kitten,
                IntakeAgeInWeeks = 5,
                Weaned = Weaned.Yes,
                RegistrationDate = new DateOnly(2024, 8, 20),
            },
            new Feline()
            {
                Name = "Crockett",
                Gender = Gender.Male,
                IntakeDate = new DateOnly(2024, 7, 3),
                Category = Category.Kitten,
                IntakeAgeInWeeks = 5,
                Weaned = Weaned.Yes,
                RegistrationDate = new DateOnly(2024, 8, 20),

            },
            new Feline()
            {
                Name = "Sweet Caroline",
                Gender = Gender.Female,
                IntakeDate = new DateOnly(2024, 7, 3),
                Category = Category.Cat,
                IntakeAgeInWeeks = null,
                Weaned = Weaned.Yes,
                RegistrationDate = new DateOnly(2024, 9, 17),
            },
            new Feline()
            {
                Name = "Tank",
                Gender = Gender.Male,
                IntakeDate = new DateOnly(2024, 7, 3),
                Category = Category.Kitten,
                IntakeAgeInWeeks = 1,
                Weaned = Weaned.Yes,
                RegistrationDate = new DateOnly(2024, 9, 17),
                Weights = new Weight[] {
                    new Weight()
                    {
                        DateTime = new DateTime(2024, 7, 14, 7, 45, 0, DateTimeKind.Utc),
                        Value = 276.0f,
                    },
                    new Weight()
                    {
                        DateTime = new DateTime(2024, 7, 15, 11, 23, 0, DateTimeKind.Utc),
                        Value = 289.0f,
                    },
                    new Weight()
                    {
                        DateTime = new DateTime(2024, 7, 16, 15, 15, 0, DateTimeKind.Utc),
                        Value = 292.0f,
                    },
                    new Weight()
                    {
                        DateTime = new DateTime(2024, 7, 16, 22, 15, 0, DateTimeKind.Utc),
                        Value = 289.0f,
                    },
                    new Weight()
                    {
                        DateTime = new DateTime(2024, 7, 17, 14, 15, 0, DateTimeKind.Utc),
                        Value = 291.0f,
                    }
                }
            },
            new Feline()
            {
                Name = "Neo",
                Gender = Gender.Male,
                IntakeDate = new DateOnly(2024, 7, 3),
                Category = Category.Kitten,
                IntakeAgeInWeeks = 1,
                Weaned = Weaned.Yes,
                RegistrationDate = new DateOnly(2024, 9, 17),
            },
            new Feline()
            {
                Name = "Trinity",
                Gender = Gender.Female,
                IntakeDate = new DateOnly(2024, 7, 3),
                Category = Category.Kitten,
                IntakeAgeInWeeks = 1,
                Weaned = Weaned.Yes,
                RegistrationDate = new DateOnly(2024, 9, 17),
            },
            new Feline()
            {
                Name = "Lady Blue",
                Gender = Gender.Female,
                IntakeDate = new DateOnly(2024, 7, 3),
                Category = Category.NursingCat,
                IntakeAgeInWeeks = null,
                Weaned = Weaned.Yes,
                RegistrationDate = new DateOnly(2024, 9, 17),
            },
            new Feline()
            {
                Name = "Link",
                Gender = Gender.Female,
                IntakeDate = new DateOnly(2024, 7, 3),
                Category = Category.NursingKitten,
                IntakeAgeInWeeks = 4,
                Weaned = Weaned.InProgress,
                RegistrationDate = new DateOnly(2024, 9, 17),
            }
        );

        await context.SaveChangesAsync();
    }
}