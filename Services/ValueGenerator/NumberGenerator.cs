namespace DevTools.Services.ValueGenerator;

public class NumberGenerator : ValueGenerator
{
    private static readonly Random _random = new();

    public override string Generate()
    {
        return _random.Next(0, 100).ToString();
    }
}
