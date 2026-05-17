using System.Globalization;
using FreeDataExportsCMD.Models;
using FreeDataExportsCMD.Services;
using FreeDataExportsv2;

namespace FreeDataExportsCMD.Providers;

internal sealed class SpreadsheetWriter : ISpreadsheetWriter
{
    public async Task SaveAsync(IReadOnlyList<string[]> rows, string path, CommandLineOptions options)
    {
        if (options.Format == OutputFormat.Ods)
        {
            var workbook = CreateOdsWorkbook(options);
            var sheet = workbook.AddWorksheet(options.SheetName);
            AddRows(sheet, rows, options.IncludeHeaderRow);
            await workbook.SaveAsync(path);
            return;
        }

        var xlsxWorkbook = CreateXlsxWorkbook(options);
        var xlsxSheet = xlsxWorkbook.AddWorksheet(options.SheetName);
        AddRows(xlsxSheet, rows, options.IncludeHeaderRow);
        await xlsxWorkbook.SaveAsync(path);
    }

    private static XlsxFile CreateXlsxWorkbook(CommandLineOptions options) =>
        new()
        {
            Creator = options.CreatedBy,
            LastModifiedBy = options.CreatedBy,
            Company = "FreeDataExportsCMD",
            Created = DateTime.Now,
            Modified = DateTime.Now
        };

    private static OdsFile CreateOdsWorkbook(CommandLineOptions options) =>
        new()
        {
            Creator = options.CreatedBy,
            LastModifiedBy = options.CreatedBy,
            Company = "FreeDataExportsCMD",
            Created = DateTime.Now,
            Modified = DateTime.Now
        };

    private static void AddRows(XlsxWorksheet sheet, IReadOnlyList<string[]> rows, bool includeHeaderRow)
    {
        var headerStyle = new CellOptions
        {
            DataType = DataType.String,
            Bold = true,
            FontColor = "FFFFFFFF",
            BackgroundColor = "FF1F4E78",
            BorderBottomColor = "FFB7DEE8",
            BorderBottomStyle = BorderStyle.Medium
        };

        var startRowIndex = includeHeaderRow ? 0 : 1;
        var rowsToWrite = rows.Skip(startRowIndex).ToArray();
        var maxColumns = rowsToWrite.Max(row => row.Length);
        var widths = new int[maxColumns];

        for (var rowIndex = startRowIndex; rowIndex < rows.Count; rowIndex++)
        {
            var row = sheet.AddRow();
            var fields = rows[rowIndex];

            for (var columnIndex = 0; columnIndex < maxColumns; columnIndex++)
            {
                var field = columnIndex < fields.Length ? fields[columnIndex] : string.Empty;
                widths[columnIndex] = Math.Max(widths[columnIndex], field.Length);

                if (includeHeaderRow && rowIndex == 0)
                {
                    row.AddCell(field, headerStyle);
                    continue;
                }

                var cell = CellInference.Infer(field);
                row.AddCell(cell.Value, cell.DataType);
            }
        }

        sheet.ColumnWidths(widths.Select(width => Math.Clamp(width + 2, 10, 60).ToString(CultureInfo.InvariantCulture)).ToArray());
    }
}
