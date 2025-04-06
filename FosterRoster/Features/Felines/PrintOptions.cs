namespace FosterRoster.Features.Felines;

using Weights;

public enum PrintSection
{
    Vitals,
    Image,
    Journal,
    Weights
}

public sealed class PrintOptions
{
    public static readonly PrintSection[] PrintSections =
    [
        PrintSection.Vitals,
        PrintSection.Image,
        PrintSection.Journal,
        PrintSection.Weights
    ];

    /// <summary>
    ///     Gets or sets the selected print sections.
    /// </summary>
    public PrintSection[] SelectedPrintSections { get; set; } = [..PrintSections];

    /// <summary>
    ///     Gets or sets the units to display for each weight.
    /// </summary>
    public WeightUnit Units { get; set; } = WeightUnit.lbs;

    /// <summary>
    ///     Convert to string representation
    /// </summary>
    /// <returns>string representation of options for Url</returns>
    public override string ToString()
    {
        Console.WriteLine(string.Join(",", SelectedPrintSections));
        Console.WriteLine(Units);
        // The default is to print everything using lbs.
        // Just include the differences in the output.
        var query = PrintSections
            .Except(SelectedPrintSections)
            .Aggregate(QueryString.Empty, (current, section) => current.Add(section.ToString(), "false"));
        if (SelectedPrintSections.Contains(PrintSection.Weights) && Units != WeightUnit.lbs)
            query = query.Add("Units", Units.ToString());
        return query.ToString();
    }
}