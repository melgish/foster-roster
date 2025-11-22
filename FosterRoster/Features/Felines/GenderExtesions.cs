namespace FosterRoster.Features.Felines;

public static class GenderExtensions
{
    extension(Gender gender)
    {
        /// <summary>
        ///     Returns the possessive form of the pronoun.
        /// </summary>
        public string PossessivePronoun
            => gender switch
            {
                Gender.Male => "his",
                Gender.Female => "her",
                _ => "their"
            };
    }
}
