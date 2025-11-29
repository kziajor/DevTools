using System.CommandLine;
using DevTools.Services.ValueGenerator;
using Spectre.Console;
using ValueType = DevTools.Services.ValueGenerator.ValueType;

namespace DevTools.Console.Commands.GenCommands.CsvCommand;

internal class CsvCommand : Command
{
    private const string SCHEMA_OPTION = "--schema";
    private const string COUNT_OPTION = "--count";
    private const string COLUMN_DIVIDER_OPTION = "--column-divider";

    public CsvCommand() : base("csv", "Generate CSV file based on schema")
    {
        Option<string> schemaOption = new(SCHEMA_OPTION)
        {
            Description = "Path to the schema file (default: schema.csv in current directory)"
        };

        Option<int> countOption = new(COUNT_OPTION)
        {
            Description = "Number of rows to generate (required)"
        };

        Option<string> columnDividerOption = new(COLUMN_DIVIDER_OPTION)
        {
            Description = "Character to use as column divider (default: ;)"
        };

        Options.Add(schemaOption);
        Options.Add(countOption);
        Options.Add(columnDividerOption);

        SetAction(Handle);
    }

    private void Handle(ParseResult result)
    {
        var schemaPath = result.GetValue<string>(SCHEMA_OPTION) ?? "schema.csv";
        var count = result.GetValue<int>(COUNT_OPTION);
        var columnDividerStr = result.GetValue<string>(COLUMN_DIVIDER_OPTION) ?? ";";
        var columnDivider = string.IsNullOrEmpty(columnDividerStr) ? ';' : columnDividerStr[0];

        if (count < 1)
        {
            AnsiConsole.MarkupLine("[red]Error:[/] Count must be greater than 0");
            return;
        }

        try
        {
            string resolvedSchemaPath = ResolvePath(schemaPath);

            if (!File.Exists(resolvedSchemaPath))
            {
                AnsiConsole.MarkupLine($"[red]Error:[/] Schema file not found: {resolvedSchemaPath}");
                return;
            }

            var lines = File.ReadAllLines(resolvedSchemaPath);

            if (lines.Length < 2)
            {
                AnsiConsole.MarkupLine("[red]Error:[/] Schema file must contain at least 2 lines (headers and types)");
                return;
            }

            // Always read schema with semicolon divider
            var headers = lines[0].Split(';');
            var typeStrings = lines[1].Split(';');

            if (headers.Length != typeStrings.Length)
            {
                AnsiConsole.MarkupLine("[red]Error:[/] Headers and types must have the same number of columns");
                return;
            }

            var valueTypes = new ValueType[typeStrings.Length];
            for (int i = 0; i < typeStrings.Length; i++)
            {
                if (!ValueGeneratorFactory.TryParseValueType(typeStrings[i].Trim(), out var valueType))
                {
                    AnsiConsole.MarkupLine($"[red]Error:[/] Unknown value type '{typeStrings[i].Trim()}' in column {i + 1}");
                    return;
                }
                valueTypes[i] = valueType;
            }

            GenerateCsvFile(resolvedSchemaPath, headers, valueTypes, count, columnDivider);

            AnsiConsole.MarkupLine($"[green]✓ CSV file generated successfully[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
        }
    }

    private string ResolvePath(string schemaPath)
    {
        if (Path.IsPathRooted(schemaPath))
        {
            return schemaPath;
        }

        return Path.Combine(Directory.GetCurrentDirectory(), schemaPath);
    }

    private void GenerateCsvFile(string schemaPath, string[] headers, ValueType[] valueTypes, int count, char columnDivider)
    {

        string directory = Path.GetDirectoryName(schemaPath) ?? Directory.GetCurrentDirectory();
        string fileName = Path.GetFileNameWithoutExtension(schemaPath);
        string outputPath = Path.Combine(directory, $"{fileName}-generated.csv");

        using (var writer = new StreamWriter(outputPath))
        {
            writer.WriteLine(string.Join(columnDivider, headers));


            for (int row = 0; row < count; row++)
            {
                var values = new string[headers.Length];
                for (int col = 0; col < headers.Length; col++)
                {
                    var generator = ValueGeneratorFactory.CreateGenerator(valueTypes[col]);
                    values[col] = generator.Generate();
                }

                writer.WriteLine(string.Join(columnDivider, values));
            }
        }
    }
}
