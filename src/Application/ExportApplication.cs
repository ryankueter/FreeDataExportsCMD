using FreeDataExportsCMD.Exceptions;
using FreeDataExportsCMD.Models;
using FreeDataExportsCMD.Providers;
using FreeDataExportsCMD.Services;

namespace FreeDataExportsCMD.Application;

internal sealed class ExportApplication(
    IInputProvider inputProvider,
    ICsvParser csvParser,
    ISpreadsheetWriter spreadsheetWriter)
{
    public async Task RunAsync(CommandLineOptions options, TextWriter statusWriter)
    {
        var csvText = await inputProvider.ReadAllAsync(options.InputFile);
        if (string.IsNullOrWhiteSpace(csvText))
        {
            throw new CommandLineException("No CSV or TSV data was provided. Pipe data into the app or use --input <file>.");
        }

        var delimiter = options.Delimiter == DelimiterKind.Auto
            ? csvParser.DetectDelimiter(csvText)
            : options.Delimiter;

        var rows = csvParser.ReadRows(csvText, delimiter);
        if (rows.Count == 0)
        {
            throw new CommandLineException("The input did not contain any rows.");
        }

        if (!options.IncludeHeaderRow && rows.Count == 1)
        {
            throw new CommandLineException("The input did not contain any data rows to export after excluding the header row.");
        }

        var savePath = options.GetSavePath();
        Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

        await spreadsheetWriter.SaveAsync(rows, savePath, options);
        statusWriter.WriteLine($"Saved {savePath}");
    }
}
