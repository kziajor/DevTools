namespace DevTools.Services.ValueGenerator;

public class GuidGenerator : ValueGenerator
{
    public override string Generate()
    {
        return System.Guid.NewGuid().ToString();
    }
}
