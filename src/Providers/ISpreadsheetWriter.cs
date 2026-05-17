using FreeDataExportsCMD.Models;

namespace FreeDataExportsCMD.Providers;

internal interface ISpreadsheetWriter
{
    Task SaveAsync(IReadOnlyList<string[]> rows, string path, CommandLineOptions options);
}
