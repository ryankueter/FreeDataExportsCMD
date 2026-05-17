using System.Globalization;
using FreeDataExportsCMD.Models;
using FreeDataExportsv2;

namespace FreeDataExportsCMD.Services;

internal static class CellInference
{
    private static readonly string[] DateTimeFormats =
    [
        "yyyy-MM-dd",
        "yyyy-MM-dd HH:mm",
        "yyyy-MM-dd HH:mm:ss",
        "M/d/yyyy",
        "M/d/yyyy h:mm tt",
        "M/d/yyyy H:mm",
        "M/d/yyyy H:mm:ss"
    ];

    public static ParsedCell Infer(string value)
    {
        var trimmed = value.Trim();

        if (trimmed.Length == 0)
        {
            return new ParsedCell(string.Empty, DataType.String);
        }

        if (bool.TryParse(trimmed, out var boolean))
        {
            return new ParsedCell(boolean, DataType.Boolean);
        }

        if (ShouldPreserveAsText(trimmed))
        {
            return new ParsedCell(value, DataType.String);
        }

        if (long.TryParse(trimmed, NumberStyles.Integer, CultureInfo.InvariantCulture, out var integer))
        {
            return new ParsedCell(integer, DataType.Integer);
        }

        if (decimal.TryParse(trimmed, NumberStyles.Number | NumberStyles.AllowCurrencySymbol, CultureInfo.InvariantCulture, out var number))
        {
            return new ParsedCell(number, DataType.Number);
        }

        if (DateTime.TryParseExact(
                trimmed,
                DateTimeFormats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces,
                out var exactDate))
        {
            return new ParsedCell(exactDate, exactDate.TimeOfDay == TimeSpan.Zero ? DataType.ShortDate : DataType.DateTime24);
        }

        if (LooksLikeDate(trimmed) &&
            DateTime.TryParse(trimmed, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out var localDate))
        {
            return new ParsedCell(localDate, localDate.TimeOfDay == TimeSpan.Zero ? DataType.ShortDate : DataType.DateTime24);
        }

        return new ParsedCell(value, DataType.String);
    }

    private static bool ShouldPreserveAsText(string value) =>
        value.Length > 1 &&
        value[0] == '0' &&
        value.All(char.IsDigit);

    private static bool LooksLikeDate(string value) =>
        value.Contains('/') || value.Contains('-');
}
