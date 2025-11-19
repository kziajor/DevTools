using System.CommandLine;

using DevTools.Console.Commands.GenCommand;
using DevTools.Console.Commands.HelloCommand;

namespace DevTools.Console;
internal static class CommandRegistrar
{
    public static void RegisterCommands(this RootCommand rootCommand)
    {
        rootCommand.Add(new HelloCommand());
        rootCommand.Add(new GenCommand());
    }
}
