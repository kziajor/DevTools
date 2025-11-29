using System.CommandLine;
using System.Runtime.InteropServices;
using System.Text;
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
            string generatedNip = NIPFaker.GenerateRandomNIP();
            AnsiConsole.MarkupLine($"[green]Generated valid NIP: {generatedNip}[/]");
            CopyToClipboard(generatedNip);
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
            var formattedNip = FormatNip(cleanNip);
            AnsiConsole.MarkupLine($"[green]NIP {formattedNip} is valid[/]");
            CopyToClipboard(formattedNip);
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
            CopyToWindowsClipboard(text);
            AnsiConsole.MarkupLine("[grey]([/][green]Copied to clipboard[/][grey])[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[grey]([/][yellow]Clipboard copy failed: {ex.Message}[/][grey])[/]");
        }
    }

    private static void CopyToWindowsClipboard(string text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        IntPtr hGlobal = Marshal.AllocHGlobal(Encoding.Unicode.GetByteCount(text) + 2);
        try
        {
            IntPtr target = hGlobal;
            foreach (char c in text)
            {
                Marshal.WriteInt16(target, c);
                target = IntPtr.Add(target, 2);
            }
            Marshal.WriteInt16(target, 0);

            if (!OpenClipboard(IntPtr.Zero))
                throw new Exception("Failed to open clipboard");

            try
            {
                EmptyClipboard();
                if (SetClipboardData(13, hGlobal) == IntPtr.Zero)
                    throw new Exception("Failed to set clipboard data");
                hGlobal = IntPtr.Zero;
            }
            finally
            {
                CloseClipboard();
            }
        }
        finally
        {
            if (hGlobal != IntPtr.Zero)
                Marshal.FreeHGlobal(hGlobal);
        }
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool CloseClipboard();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetClipboardData(uint format, IntPtr hMem);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool EmptyClipboard();
}
