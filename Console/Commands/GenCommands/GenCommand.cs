using System.CommandLine;

namespace DevTools.Console.Commands.GenCommand;

public class GenCommand : Command
{
    public GenCommand() : base("gen", "Generate random data")
    {
        Add(new EmailCommand.EmailCommand());
        Add(new NIPCommand.NIPCommand());
        Add(new NumberCommand.NumberCommand());
        Add(new RegonCommand.RegonCommand());
        Add(new GuidCommand.GuidCommand());
    }
}
