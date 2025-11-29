using Bogus;

namespace DevTools.Services.ValueGenerator;

public class StringGenerator : ValueGenerator
{
    private static readonly Faker _faker = new();

    public override string Generate()
    {
        return _faker.Lorem.Word();
    }
}
