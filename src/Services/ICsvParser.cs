using FreeDataExportsCMD.Models;

namespace FreeDataExportsCMD.Services;

internal interface ICsvParser
{
    DelimiterKind DetectDelimiter(string text);

    List<string[]> ReadRows(string text, DelimiterKind delimiter);
}
