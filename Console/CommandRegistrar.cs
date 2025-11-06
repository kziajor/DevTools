using System.CommandLine;
using DevTools.Console.Commands.HelloCommand;
using DevTools.Console.Commands.NumberCommand;

namespace DevTools.Console;
internal static class CommandRegistrar
{
    public static void RegisterCommands(this RootCommand rootCommand)
    {
        rootCommand.Add(new HelloCommand());
        rootCommand.Add(new NumberCommand());
    }
}
