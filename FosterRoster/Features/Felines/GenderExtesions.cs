namespace FosterRoster.Features.Felines;

public static class GenderExtensions
{
    /// <summary>
    ///     Returns the possessive form of the pronoun.
    /// </summary>
    public static string PossessivePronoun(this Gender gender)
    {
        return gender switch
        {
            Gender.Male => "his",
            Gender.Female => "her",
            _ => "their"
        };
    }
}