namespace DevTools.Services.ValueGenerator;

public class NipGenerator : ValueGenerator
{
    public override string Generate()
    {
        return NIPFaker.GenerateRandomNIP();
    }
}
