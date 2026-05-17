namespace FreeDataExportsCMD.Cli;

internal static class Usage
{
    public static void Write(TextWriter writer)
    {
        writer.WriteLine("""
FreeDataExportsCMD
Convert CSV or TSV piped on stdin into an .xlsx or .ods spreadsheet.

Usage:
  FreeDataExportsCMD [options]

Options:
  -i, --input <file>           Read CSV/TSV from a file instead of stdin.
  -o, --output <path>          Full output file path, or a directory path.
  -p, --path <directory>       Directory where the spreadsheet will be saved. Defaults to current directory.
  -n, --name <file-name>       Spreadsheet file name. Extension is optional. Defaults to export.
  -f, --format <xlsx|ods>      Spreadsheet format. Defaults to xlsx unless --name/--output ends in .ods.
  -d, --delimiter <auto|comma|tab>
                               Input delimiter. Defaults to auto.
  -s, --sheet, --sheetname <name>
                               Worksheet tab name. Defaults to Data.
      --createdby <name>       Creator metadata written to the spreadsheet. Defaults to FreeDataExportsCMD.
  -h, --help                   Show help.

Examples:
  some-report-command | FreeDataExportsCMD --format xlsx --name report.xlsx --path "C:\mydir" --sheetname Orders
  some-report-command | FreeDataExportsCMD --output "C:\mydir\report.ods"
""");
    }
}
