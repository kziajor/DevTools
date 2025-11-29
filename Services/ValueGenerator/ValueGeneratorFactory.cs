namespace DevTools.Services.ValueGenerator;

public class ValueGeneratorFactory
{
    public static ValueGenerator CreateGenerator(ValueType valueType)
    {
        return valueType switch
        {
            ValueType.Number => new NumberGenerator(),
            ValueType.String => new StringGenerator(),
            ValueType.FirstName => new FirstNameGenerator(),
            ValueType.LastName => new LastNameGenerator(),
            ValueType.Email => new EmailGenerator(),
            ValueType.Nip => new NipGenerator(),
            ValueType.Regon => new RegonGenerator(),
            ValueType.Guid => new GuidGenerator(),
            _ => throw new ArgumentException($"Unknown value type: {valueType}", nameof(valueType))
        };
    }

    public static bool TryParseValueType(string typeString, out ValueType valueType)
    {
        return Enum.TryParse<ValueType>(typeString, ignoreCase: true, out valueType);
    }
}
