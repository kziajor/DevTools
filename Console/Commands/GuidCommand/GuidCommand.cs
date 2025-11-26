using System.CommandLine;
using Spectre.Console;
using TextCopy;

namespace DevTools.Console.Commands.GuidCommand;

public class GuidCommand : Command
{
    private const string COUNT_OPTION = "--count";

    public GuidCommand() : base("guid", "Generates random GUID(s) (Globally Unique Identifier)")
    {
        Option<int> countOption = new(COUNT_OPTION)
        {
            Description = "Number of GUIDs to generate"
        };

        Options.Add(countOption);

        SetAction(Handle);
    }

    private void Handle(ParseResult result)
    {
        var count = result.GetValue<int>(COUNT_OPTION);
        count = count == default ? 1 : count;

        if (count < 1)
        {
            AnsiConsole.MarkupLine("[red]Error:[/] Count must be greater than 0");
            return;
        }

        if (count == 1)
        {
            string guid = Guid.NewGuid().ToString();
            var figletGuid = new FigletText(guid)
       .Centered()
      .Color(Color.Green);

            AnsiConsole.Write(figletGuid);
            CopyToClipboard(guid);
            AnsiConsole.MarkupLine("[grey]([/][green]Copied to clipboard[/][grey])[/]");
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                AnsiConsole.WriteLine(Guid.NewGuid().ToString());
            }
        }
    }

    private async void CopyToClipboard(string text)
    {
        try
        {
            await ClipboardService.SetTextAsync(text);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[grey]([/][yellow]Clipboard copy failed: {ex.Message}[/][grey])[/]");
        }
    }
}
