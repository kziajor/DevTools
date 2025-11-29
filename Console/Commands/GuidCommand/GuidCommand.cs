using System.CommandLine;
using System.Runtime.InteropServices;
using System.Text;
using Spectre.Console;

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
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                AnsiConsole.WriteLine(Guid.NewGuid().ToString());
            }
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

