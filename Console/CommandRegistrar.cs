using System.CommandLine;

using DevTools.Console.Commands.EmailCommand;
using DevTools.Console.Commands.NIPCommand;
using DevTools.Console.Commands.HelloCommand;
using DevTools.Console.Commands.NumberCommand;
using DevTools.Console.Commands.RegonCommand;

namespace DevTools.Console;
internal static class CommandRegistrar
{
    public static void RegisterCommands(this RootCommand rootCommand)
    {
        rootCommand.Add(new HelloCommand());
        rootCommand.Add(new EmailCommand());
        rootCommand.Add(new NIPCommand());
        rootCommand.Add(new NumberCommand());
        rootCommand.Add(new RegonCommand());
    }
}
