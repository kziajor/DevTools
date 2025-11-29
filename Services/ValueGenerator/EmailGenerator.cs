namespace DevTools.Services.ValueGenerator;

public class EmailGenerator : ValueGenerator
{
    public override string Generate()
    {
        return InternetFaker.GenerateRandomEmail();
    }
}
