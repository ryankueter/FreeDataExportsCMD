# FreeDataExportsCMD

FreeDataExportsCMD is a .NET tool that converts CSV or TSV text into `.xlsx` or `.ods` spreadsheet files using [FreeDataExportsv2](https://www.nuget.org/packages/FreeDataExportsv2).

It is designed for command-line pipelines: one command writes comma-delimited or tab-delimited data to stdout, and `FreeDataExportsCMD` reads that data from stdin and saves a spreadsheet.

> Use a single pipe character, `|`, to pass CSV/TSV into this tool. In most shells, `||` means "run the next command only if the first command fails."

## Install

```powershell
dotnet tool install --global FreeDataExportsCMD
```

Update an existing installation:

```powershell
dotnet tool update --global FreeDataExportsCMD
```

After installation, run the tool with:

```powershell
FreeDataExportsCMD --help
```

## Basic Usage

Pipe CSV into the tool and create an Excel workbook in the current directory:

```powershell
some-report-command | FreeDataExportsCMD --format xlsx --name report.xlsx
```

Create an OpenDocument spreadsheet:

```powershell
some-report-command | FreeDataExportsCMD --format ods --name report.ods
```

Save to a specific folder:

```powershell
some-report-command | FreeDataExportsCMD --format xlsx --name report.xlsx --path "C:\mydir"
```

Name the worksheet tab and set creator metadata:

```powershell
some-report-command | FreeDataExportsCMD --format xlsx --name report.xlsx --path "C:\mydir" --sheetname Orders --createdby "Contoso Reports"
```

Save to a full file path:

```powershell
some-report-command | FreeDataExportsCMD --output "C:\mydir\report.ods"
```

Read from an existing CSV file instead of stdin:

```powershell
FreeDataExportsCMD --input "C:\mydir\data.csv" --output "C:\mydir\data.xlsx"
```

## Options

```text
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
```

## Notes

- Comma-delimited and tab-delimited input are supported.
- Quoted CSV fields are supported, including escaped quotes.
- The first input row is treated as a header row and styled in the spreadsheet.
- Use `--sheetname` to control the worksheet tab name.
- Use `--createdby` to set spreadsheet creator metadata.
- Values are lightly inferred as numbers, dates, booleans, or text. Numeric strings with leading zeroes are preserved as text.
- Status messages are written to stderr so stdout stays available for pipeline use.
