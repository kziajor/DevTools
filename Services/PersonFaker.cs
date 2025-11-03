using Bogus;

namespace DevTools.Services;

public static class PersonFaker
{
    public static string FullName()
    {
        Faker faker = new("en");

        return faker.Name.FullName();
    }
}
