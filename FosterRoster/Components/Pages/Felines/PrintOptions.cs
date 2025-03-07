namespace FosterRoster.Components.Pages.Felines;

using System.ComponentModel;
using System.Text.Json.Serialization;

public enum PrintSection
{
    Vitals,
    Image,
    Journal,
    Weights
}

public sealed class PrintOptions
{
    public static readonly PrintSection[] PrintSections = [
        PrintSection.Vitals,
        PrintSection.Image,
        PrintSection.Journal,
        PrintSection.Weights
    ];

    public PrintSection[] SelectedPrintSections { get; set; } = [..PrintSections];

    public WeightUnit Units { get; set; } = WeightUnit.lbs;

    /// <summary>
    /// Convert to string representation
    /// </summary>
    /// <returns>string representation of options for Url</returns>
    public override string ToString()
    {
        var query = PrintSections
            .Except(SelectedPrintSections)
            .Aggregate(QueryString.Empty, (current, section) => current.Add(section.ToString(), "false"));
        if (SelectedPrintSections.Contains(PrintSection.Weights) && Units != WeightUnit.lbs)
        {
            query.Add("Units", Units.ToString());
        }
        return query.ToString();
    }
}