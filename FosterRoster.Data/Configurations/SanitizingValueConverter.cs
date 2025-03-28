using Ganss.Xss;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FosterRoster.Data.Configurations;

internal sealed class SanitizingValueConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<string, string>(
        v => Sanitizer.Sanitize(v.Trim(), string.Empty, null),
        v => Sanitizer.Sanitize(v.Trim(), string.Empty, null),
        mappingHints)
{
    private static readonly HtmlSanitizer Sanitizer = new();
}