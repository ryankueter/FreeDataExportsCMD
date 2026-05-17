using FreeDataExportsCMD.Models;
using Microsoft.VisualBasic.FileIO;

namespace FreeDataExportsCMD.Services;

internal sealed class CsvParser : ICsvParser
{
    public DelimiterKind DetectDelimiter(string text)
    {
        var firstDataLine = text
            .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault() ?? string.Empty;

        var commaCount = CountDelimiterOutsideQuotes(firstDataLine, ',');
        var tabCount = CountDelimiterOutsideQuotes(firstDataLine, '\t');

        return tabCount > commaCount ? DelimiterKind.Tab : DelimiterKind.Comma;
    }

    public List<string[]> ReadRows(string text, DelimiterKind delimiter)
    {
        using var reader = new StringReader(text);
        using var parser = new TextFieldParser(reader)
        {
            TextFieldType = FieldType.Delimited,
            HasFieldsEnclosedInQuotes = true,
            TrimWhiteSpace = false
        };

        parser.SetDelimiters(delimiter == DelimiterKind.Tab ? "\t" : ",");

        var rows = new List<string[]>();
        while (!parser.EndOfData)
        {
            var fields = parser.ReadFields();
            if (fields is null)
            {
                continue;
            }

            if (fields.Length == 1 && string.IsNullOrWhiteSpace(fields[0]))
            {
                continue;
            }

            rows.Add(fields);
        }

        return rows;
    }

    private static int CountDelimiterOutsideQuotes(string line, char delimiter)
    {
        var count = 0;
        var inQuotes = false;

        for (var i = 0; i < line.Length; i++)
        {
            var current = line[i];

            if (current == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    i++;
                    continue;
                }

                inQuotes = !inQuotes;
                continue;
            }

            if (!inQuotes && current == delimiter)
            {
                count++;
            }
        }

        return count;
    }
}
