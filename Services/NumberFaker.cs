namespace DevTools.Services;
public static class NumberFaker
{
    private static readonly Random _random = new();
    public static int RandomInt(int min = 0, int max = 100)
    {
        return _random.Next(min, max);
    }
}
