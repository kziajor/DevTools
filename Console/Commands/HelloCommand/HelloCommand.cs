using System.CommandLine;
using DevTools.Services;
using Spectre.Console;

namespace DevTools.Console.Commands.HelloCommand;

internal class HelloCommand : Command
{
    private const string NAME_OPTION = "--name";
    public HelloCommand() : base("hello", "Says hello to the user.")
    {
        Option<string> nameOption = new(NAME_OPTION)
        {
            Description = "The name of the user to greet.",
            Required = false
        };

        Options.Add(nameOption);

        SetAction(Handle);
    }

    private void Handle(ParseResult result)
    {
        string? name = result.GetValue<string>(NAME_OPTION);

        name ??= PersonFaker.FullName();

        FigletText figletHello = new FigletText("Hello")
            .Centered()
            .Color(Color.White);

        FigletText figletName = new FigletText(name)
            .Centered()
            .Color(Color.DodgerBlue3);

        AnsiConsole.Write(figletHello);
        AnsiConsole.Write(figletName);
    }
}
