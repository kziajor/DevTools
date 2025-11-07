namespace DevTools.Services;
public static class RegonFaker
{
    private static readonly Random _random = new();

    public static string RandomRegon(int length = 9)
    {
        if (length != 9 && length != 14)
            throw new ArgumentException("REGON length must be either 9 or 14", nameof(length));

        if (length == 9)
            return GenerateRegon9();
        return GenerateRegon14();
    }

    private static string GenerateRegon9()
    {
        int[] weights = new int[] { 8, 9, 2, 3, 4, 5, 6, 7 };
        int[] digits = new int[9];

        for (int i = 0; i < 8; i++)
        {
            digits[i] = _random.Next(0, 10);
        }

        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += digits[i] * weights[i];
        }
        int checksum = sum % 11;
        if (checksum == 10)
        {
            checksum = 0;
        }
        digits[8] = checksum;

        return string.Concat(digits);
    }

    private static string GenerateRegon14()
    {
        int[] weights = new int[] { 2, 4, 8, 5, 0, 9, 7, 3, 6, 1, 2, 4, 8 };
        int[] digits = new int[14];


        string regon9 = GenerateRegon9();
        for (int i = 0; i < 9; i++)
        {
            digits[i] = regon9[i] - '0';
        }


        for (int i = 9; i < 13; i++)
        {
            digits[i] = _random.Next(0, 10);
        }


        int sum = 0;
        for (int i = 0; i < 13; i++)
        {
            sum += digits[i] * weights[i];
        }
        int checksum = sum % 11;
        if (checksum == 10)
        {
            checksum = 0;
        }
        digits[13] = checksum;

        return string.Concat(digits);
    }
}
