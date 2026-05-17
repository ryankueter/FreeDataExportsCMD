namespace FreeDataExportsCMD.Models;

internal sealed record CommandLineOptions
{
    public bool ShowHelp { get; init; }
    public string? InputFile { get; init; }
    public string? OutputPath { get; init; }
    public string DirectoryPath { get; init; } = Directory.GetCurrentDirectory();
    public string FileName { get; init; } = "export";
    public string SheetName { get; init; } = "Data";
    public string CreatedBy { get; init; } = "FreeDataExportsCMD";
    public OutputFormat Format { get; init; } = OutputFormat.Xlsx;
    public DelimiterKind Delimiter { get; init; } = DelimiterKind.Auto;

    public string GetSavePath()
    {
        if (!string.IsNullOrWhiteSpace(OutputPath))
        {
            var fullOutputPath = Path.GetFullPath(OutputPath);
            var extension = Path.GetExtension(fullOutputPath);

            if (string.IsNullOrWhiteSpace(extension))
            {
                var fileName = EnsureExtension(FileName, Format);
                return Path.Combine(fullOutputPath, fileName);
            }

            return fullOutputPath;
        }

        var directory = Path.GetFullPath(DirectoryPath);
        return Path.Combine(directory, EnsureExtension(FileName, Format));
    }

    private static string EnsureExtension(string fileName, OutputFormat format)
    {
        var expectedExtension = format == OutputFormat.Ods ? ".ods" : ".xlsx";
        var currentExtension = Path.GetExtension(fileName);

        return string.IsNullOrWhiteSpace(currentExtension)
            ? fileName + expectedExtension
            : Path.ChangeExtension(fileName, expectedExtension);
    }
}
