using FreeDataExportsCMD.Exceptions;
using FreeDataExportsCMD.Models;

namespace FreeDataExportsCMD.Cli;

internal static class CommandLineParser
{
    public static CommandLineOptions Parse(string[] args)
    {
        var options = new CommandLineOptions();

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];

            switch (arg)
            {
                case "-h":
                case "--help":
                    options = options with { ShowHelp = true };
                    break;
                case "-i":
                case "--input":
                    options = options with { InputFile = ReadValue(args, ref i, arg) };
                    break;
                case "-o":
                case "--output":
                    options = options with { OutputPath = ReadValue(args, ref i, arg) };
                    break;
                case "-p":
                case "--path":
                    options = options with { DirectoryPath = ReadValue(args, ref i, arg) };
                    break;
                case "-n":
                case "--name":
                    options = options with { FileName = ReadValue(args, ref i, arg) };
                    break;
                case "-s":
                case "--sheet":
                case "--sheetname":
                    options = options with { SheetName = ReadValue(args, ref i, arg) };
                    break;
                case "--createdby":
                    options = options with { CreatedBy = ReadValue(args, ref i, arg) };
                    break;
                case "-f":
                case "--format":
                    options = options with { Format = ParseFormat(ReadValue(args, ref i, arg)) };
                    break;
                case "-d":
                case "--delimiter":
                    options = options with { Delimiter = ParseDelimiter(ReadValue(args, ref i, arg)) };
                    break;
                default:
                    throw new CommandLineException($"Unknown option '{arg}'.");
            }
        }

        options = options with
        {
            Format = ResolveFormat(options),
            FileName = Path.GetFileName(options.FileName)
        };

        Validate(options);
        return options;
    }

    private static void Validate(CommandLineOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.FileName))
        {
            throw new CommandLineException("The file name cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(options.SheetName))
        {
            throw new CommandLineException("The sheet name cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(options.CreatedBy))
        {
            throw new CommandLineException("The created by value cannot be empty.");
        }
    }

    private static OutputFormat ResolveFormat(CommandLineOptions options)
    {
        var pathToInspect = !string.IsNullOrWhiteSpace(options.OutputPath)
            ? options.OutputPath
            : options.FileName;

        return Path.GetExtension(pathToInspect).ToLowerInvariant() switch
        {
            ".ods" => OutputFormat.Ods,
            ".xlsx" => OutputFormat.Xlsx,
            _ => options.Format
        };
    }

    private static string ReadValue(string[] args, ref int index, string optionName)
    {
        if (index + 1 >= args.Length || args[index + 1].StartsWith('-'))
        {
            throw new CommandLineException($"{optionName} requires a value.");
        }

        index++;
        return args[index];
    }

    private static OutputFormat ParseFormat(string value) =>
        value.Trim().ToLowerInvariant() switch
        {
            "xlsx" => OutputFormat.Xlsx,
            ".xlsx" => OutputFormat.Xlsx,
            "ods" => OutputFormat.Ods,
            ".ods" => OutputFormat.Ods,
            _ => throw new CommandLineException("--format must be xlsx or ods.")
        };

    private static DelimiterKind ParseDelimiter(string value) =>
        value.Trim().ToLowerInvariant() switch
        {
            "auto" => DelimiterKind.Auto,
            "comma" => DelimiterKind.Comma,
            "," => DelimiterKind.Comma,
            "tab" => DelimiterKind.Tab,
            "tsv" => DelimiterKind.Tab,
            "\\t" => DelimiterKind.Tab,
            _ => throw new CommandLineException("--delimiter must be auto, comma, or tab.")
        };
}
