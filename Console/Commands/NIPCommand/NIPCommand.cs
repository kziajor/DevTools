using System.CommandLine;
using DevTools.Services;
using Spectre.Console;

namespace DevTools.Console.Commands.NIPCommand;

public class NIPCommand : Command
{
    private readonly Argument<string?> _nipArgument;
    private const string NIP_ARGUMENT = "nip";
    private const string COUNT_OPTION = "--count";

    public NIPCommand() : base("nip", "Validates Polish Tax Identification Number (NIP)")
    {
        _nipArgument = new Argument<string?>(NIP_ARGUMENT)
        {
            Description = "NIP number to validate (can include or omit hyphens). If not provided, generates a valid NIP.",
            Arity = ArgumentArity.ZeroOrOne
        };

        Add(_nipArgument);

        Option<int> countOption = new(COUNT_OPTION)
        {
            Description = "Number of NIP numbers to generate (only when no NIP is provided for validation)"
        };

        Options.Add(countOption);

        SetAction(Handle);
    }

    private void Handle(ParseResult result)
    {
        string? inputNip = result.GetValue(_nipArgument);

        if (!string.IsNullOrEmpty(inputNip))
        {
            ValidateNip(inputNip);
            return;
        }

        var count = result.GetValue<int>(COUNT_OPTION);
        count = count == default ? 1 : count;

        if (count < 1)
        {
            AnsiConsole.MarkupLine("[red]Error:[/] Count must be greater than 0");
            return;
        }

        if (count == 1)
        {
            string generatedNip = NIPFaker.GenerateRandomNIP();
            var figletNip = new FigletText(generatedNip)
                .Centered()
                .Color(Color.Green);

            AnsiConsole.Write(figletNip);
            CopyToClipboard(generatedNip);
            AnsiConsole.MarkupLine("[grey]([/][green]Copied to clipboard[/][grey])[/]");
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                string generatedNip = NIPFaker.GenerateRandomNIP();
                AnsiConsole.WriteLine(generatedNip);
            }
        }
    }

    private void ValidateNip(string inputNip)
    {
        var cleanNip = inputNip.Replace("-", "").Replace(" ", "");

        if (!IsValidNipFormat(cleanNip))
        {
            AnsiConsole.MarkupLine("[red]Invalid NIP format. NIP should be 10 digits.[/]");
            return;
        }

        if (IsValidNip(cleanNip))
        {
            AnsiConsole.MarkupLine($"[green]NIP {FormatNip(cleanNip)} is valid[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]NIP {FormatNip(cleanNip)} is invalid[/]");
        }
    }

    private static bool IsValidNipFormat(string nip)
    {
        return nip.Length == 10 && nip.All(char.IsDigit);
    }

    private static bool IsValidNip(string nip)
    {
        int[] weights = { 6, 5, 7, 2, 3, 4, 5, 6, 7 };
        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            sum += (nip[i] - '0') * weights[i];
        }

        int checksum = sum % 11;
        if (checksum == 10)
            return false;

        return checksum == (nip[9] - '0');
    }

    private static string FormatNip(string nip)
    {
        return $"{nip[..3]}-{nip.Substring(3, 3)}-{nip.Substring(6, 2)}-{nip.Substring(8, 2)}";
    }

    private void CopyToClipboard(string text)
    {
        try
        {
            if (OperatingSystem.IsWindows())
            {
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "powershell",
                        Arguments = $"-Command \"'{text}' | Set-Clipboard\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                process.WaitForExit();
            }
            else if (OperatingSystem.IsLinux())
            {
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "xclip",
                        Arguments = "-selection clipboard",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardInput = true
                    }
                };
                process.Start();
                using (var sw = process.StandardInput)
                {
                    sw.WriteLine(text);
                }
                process.WaitForExit();
            }
            else if (OperatingSystem.IsMacOS())
            {
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "pbcopy",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardInput = true
                    }
                };
                process.Start();
                using (var sw = process.StandardInput)
                {
                    sw.WriteLine(text);
                }
                process.WaitForExit();
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[grey]([/][yellow]Clipboard copy failed: {ex.Message}[/][grey])[/]");
        }
    }
}
