using System.CommandLine;
using DevTools.Services;
using Spectre.Console;
using TextCopy;

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
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]✗ REGON {regon} is [bold]INVALID[/][/]");
        }
    }
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
