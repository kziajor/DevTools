using System.CommandLine;
using System.CommandLine.Parsing;
using DevTools.Console;

RootCommand rootCommand = new()
{
    Description = "DevTools Console Application"
};

rootCommand.RegisterCommands();

ParseResult parseResult = rootCommand.Parse(args);

if (parseResult.Errors.Count == 0)
{
    return parseResult.Invoke();
}

foreach (ParseError parseError in parseResult.Errors)
{
    Console.Error.WriteLine(parseError.Message);
}
return 1;