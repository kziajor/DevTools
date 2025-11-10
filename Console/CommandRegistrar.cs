using System.CommandLine;
using DevTools.Console.Commands.EmailCommand;
using DevTools.Console.Commands.HelloCommand;
using DevTools.Console.Commands.NIPCommand;

namespace DevTools.Console;
internal static class CommandRegistrar
{
    public static void RegisterCommands(this RootCommand rootCommand)
    {
        rootCommand.Add(new HelloCommand());
        rootCommand.Add(new EmailCommand());
        rootCommand.Add(new NIPCommand());
    }
}
