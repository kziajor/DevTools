using Bogus;

namespace DevTools.Services.ValueGenerator;

public class FirstNameGenerator : ValueGenerator
{
    private static readonly Faker _faker = new("pl");

    public override string Generate()
    {
        return _faker.Name.FirstName();
    }
}
