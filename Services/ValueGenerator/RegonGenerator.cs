namespace DevTools.Services.ValueGenerator;

public class RegonGenerator : ValueGenerator
{
    public override string Generate()
    {
        return RegonFaker.RandomRegon();
    }
}
