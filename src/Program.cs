using FreeDataExportsCMD.Application;
using FreeDataExportsCMD.Cli;
using FreeDataExportsCMD.Exceptions;
using FreeDataExportsCMD.Providers;
using FreeDataExportsCMD.Services;

try
{
    var options = CommandLineParser.Parse(args);

    if (options.ShowHelp)
    {
        Usage.Write(Console.Out);
        return 0;
    }

    var application = new ExportApplication(
        new InputProvider(),
        new CsvParser(),
        new SpreadsheetWriter());

    await application.RunAsync(options, Console.Error);
    return 0;
}
catch (CommandLineException ex)
{
    Console.Error.WriteLine(ex.Message);
    Usage.Write(Console.Error);
    return 1;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Export failed: {ex.Message}");
    return 1;
}
