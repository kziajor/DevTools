namespace DevTools.Services;

public static class NIPFaker
{
    private static readonly Random _random = new();

    public static string GenerateRandomNIP()
    {
        int officeCode = _random.Next(101, 999);

        int middlePart = _random.Next(10000, 100000);
        int lastDigit = _random.Next(0, 10);

        string baseNip = $"{officeCode:D3}{middlePart:D5}{lastDigit}";

        int[] weights = { 6, 5, 7, 2, 3, 4, 5, 6, 7 };
        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            sum += (baseNip[i] - '0') * weights[i];
        }

        int checkDigit = sum % 11;

        if (checkDigit == 10)
        {
            return GenerateRandomNIP();
        }

        string fullNip = baseNip + checkDigit;

        if (!IsValidNip(fullNip))
        {
            return GenerateRandomNIP();
        }

        return FormatNip(fullNip);
    }

    public static string GenerateRandomInvalidNIP()
    {
        string validNip = GenerateRandomNIP().Replace("-", "");

        int currentLastDigit = validNip[9] - '0';
        int newLastDigit;
        do
        {
            newLastDigit = _random.Next(0, 10);
        } while (newLastDigit == currentLastDigit);

        string invalidNip = validNip[..9] + newLastDigit;
        return FormatNip(invalidNip);
    }

    private static bool IsValidNip(string nip)
    {
        int[] weights = { 6, 5, 7, 2, 3, 4, 5, 6, 7 };
        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            sum += (nip[i] - '0') * weights[i];
        }

        int checksum = sum % 11;
        if (checksum == 10)
            return false;

        return checksum == (nip[9] - '0');
    }

    private static string FormatNip(string nip)
    {
        return $"{nip[..3]}-{nip.Substring(3, 3)}-{nip.Substring(6, 2)}-{nip.Substring(8, 2)}";
    }
}
