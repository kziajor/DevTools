using System.CommandLine;
using DevTools.Services;
using Spectre.Console;

namespace DevTools.Console.Commands.NumberCommand;
internal class NumberCommand : Command
{
    public NumberCommand() : base("number", "A command that does nothing with numbers.")
    {
        Argument<int> minValue = new("number")
        {
            Description = "An integer number.",
            Arity = ArgumentArity.ExactlyOne,
            DefaultValueFactory = result => 1
        };
        Arguments.Add(minValue);
        Argument<int> maxValue = new("max")
        {
            Description = "An integer number.",
            Arity = ArgumentArity.ExactlyOne,
            DefaultValueFactory = result => 100
        };
        Arguments.Add(maxValue);
        SetAction(Handle);
    }
    private void Handle(ParseResult result)
    {
        int min = result.GetRequiredValue<int>("number");
        int max = result.GetRequiredValue<int>("max");
        int randomNumber = NumberFaker.RandomInt(min, max);

        FigletText figletNumber = new FigletText(randomNumber.ToString())
            .Centered()
            .Color(Color.White);

        AnsiConsole.Write(figletNumber);
    }
}
