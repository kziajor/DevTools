using System.CommandLine;
using DevTools.Console.Commands.EmailCommand;
using DevTools.Console.Commands.NIPCommand;
using DevTools.Console.Commands.NumberCommand;
using DevTools.Console.Commands.RegonCommand;

namespace DevTools.Console.Commands.GenCommand;

public class GenCommand : Command
{
    public GenCommand() : base("gen", "Generate random data")
    {
        Add(new EmailCommand.EmailCommand());
        Add(new NIPCommand.NIPCommand());
        Add(new NumberCommand.NumberCommand());
        Add(new RegonCommand.RegonCommand());
    }
}
