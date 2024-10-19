namespace FosterRoster.Data.Configurations;

using Ganss.Xss;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

internal sealed class SanitizingValueConverter : ValueConverter<string, string>
{
    private static readonly IHtmlSanitizer sanitizer = new HtmlSanitizer();

    public SanitizingValueConverter(ConverterMappingHints? mappingHints = null)
        : base(
            v => sanitizer.Sanitize(v.Trim(), string.Empty, null),
            v => sanitizer.Sanitize(v.Trim(), string.Empty, null),
            mappingHints)
    { }
}