using Bogus;

namespace DevTools.Services;
public static class InternetFaker
{
    private static readonly Faker _faker = new();

    public static string GenerateRandomEmail(string? domain = "example.com")
    {
        if (string.IsNullOrEmpty(domain) || domain == "example.com")
        {
            return _faker.Internet.Email();
        }

        string username = _faker.Internet.UserName();
        return $"{username}@{domain}";
    }
}
