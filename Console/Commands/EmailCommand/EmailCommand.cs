using System.CommandLine;
using DevTools.Services;
using Spectre.Console;

namespace DevTools.Console.Commands.EmailCommand;
internal class EmailCommand : Command
{
    public EmailCommand() : base("email", "Generate random Email")
    {
        Argument<string> domainArgument = new("domain")
        {
            Description = "Domain for generated email.",
            Arity = ArgumentArity.ZeroOrOne,
            DefaultValueFactory = (result) => "example.com"
        };

        Arguments.Add(domainArgument);

        SetAction(Handle);
    }

    private void Handle(ParseResult result)
    {
        string? domain = result.GetValue<string>("domain");

        string email = InternetFaker.GenerateRandomEmail(domain);


        FigletText figletEmail = new FigletText(email)
            .Centered()
            .Color(Color.DodgerBlue3);

        AnsiConsole.Write(figletEmail);
    }
}
