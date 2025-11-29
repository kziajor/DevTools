using Bogus;

namespace DevTools.Services.ValueGenerator;

public class LastNameGenerator : ValueGenerator
{
    private static readonly Faker _faker = new("pl");

    public override string Generate()
    {
        return _faker.Name.LastName();
    }
}
