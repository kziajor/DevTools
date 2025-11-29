using System.CommandLine;
using System.Runtime.InteropServices;
using System.Text;
using DevTools.Services;
using Spectre.Console;

namespace DevTools.Console.Commands.RegonCommand;

internal class RegonCommand : Command
{
    private const string LENGTH_OPTION = "--length";
    private const string COUNT_OPTION = "--count";
    private const string TABLE_OPTION = "--table";

    public RegonCommand() : base("regon", "Generates random valid REGON number(s) or validates a REGON")
    {
        Argument<string?> regonArgument = new("regon")
        {
            Description = "REGON number to validate (optional)",
            Arity = ArgumentArity.ZeroOrOne
        };

        Arguments.Add(regonArgument);

        Option<int> lengthOption = new(LENGTH_OPTION)
        {
            Description = "Length of REGON number (9 or 14)"
        };

        Option<int> countOption = new(COUNT_OPTION)
        {
            Description = "Number of REGON numbers to generate"
        };

        Option<bool> tableOption = new(TABLE_OPTION)
        {
            Description = "Display results in a table format"
        };

        Options.Add(lengthOption);
        Options.Add(countOption);
        Options.Add(tableOption);

        SetAction(Handle);
    }

    private void Handle(ParseResult result)
    {
        var regonToValidate = result.GetValue<string?>("regon");

        if (!string.IsNullOrEmpty(regonToValidate))
        {
            ValidateUserRegon(regonToValidate);
            return;
        }

        var length = result.GetValue<int>(LENGTH_OPTION);
        length = length == default ? 9 : length;

        var count = result.GetValue<int>(COUNT_OPTION);
        count = count == default ? 1 : count;

        var useTable = result.GetValue<bool>(TABLE_OPTION);

        if (length != 9 && length != 14)
        {
            AnsiConsole.MarkupLine("[red]Error:[/] REGON length must be either 9 or 14");
            return;
        }

        if (count < 1)
        {
            AnsiConsole.MarkupLine("[red]Error:[/] Count must be greater than 0");
            return;
        }

        if (useTable)
        {
            var table = new Table()
               .AddColumn("REGON")
             .Border(TableBorder.Rounded);

            for (int i = 0; i < count; i++)
            {
                table.AddRow(RegonFaker.RandomRegon(length));
            }

            AnsiConsole.Write(table);
        }
        else if (count == 1)
        {
            string regon = RegonFaker.RandomRegon(length);
            var figletRegon = new FigletText(regon)
           .Centered()
                    .Color(Color.White);

            AnsiConsole.Write(figletRegon);
            CopyToClipboard(regon);
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                AnsiConsole.WriteLine(RegonFaker.RandomRegon(length));
            }
        }
    }

    private void ValidateUserRegon(string regon)
    {
        if (RegonFaker.IsValidRegon(regon))
        {
            AnsiConsole.MarkupLine($"[green]✓ REGON {regon} is [bold]VALID[/][/]");
            CopyToClipboard(regon);
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]✗ REGON {regon} is [bold]INVALID[/][/]");
        }
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

