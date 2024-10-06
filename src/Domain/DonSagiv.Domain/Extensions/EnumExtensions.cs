using System.ComponentModel;

namespace DonSagiv.Domain.Extensions;

public static class EnumExtensions
{
    public static string? GetDescription<TEnum>(this TEnum value)
        where TEnum : Enum
    {
        return GetDescription(value);
    }

    public static string GetDescription(this Enum value)
    {
        var type = value.GetType();

        return GetEnumValueString(value, type);
    }

    private static string GetEnumValueString<TEnum>(TEnum value, Type type)
        where TEnum : Enum
    {
        var memInfo = type.GetMember(type.GetEnumName(value)!);

        var descriptionAttribute = memInfo
            .First()
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .OfType<DescriptionAttribute>()
            .FirstOrDefault();

        if(descriptionAttribute is null)
        {
            return value.ToString();
        }

        return descriptionAttribute.Description;
    }

    public static TEnum[] GetAllValues<TEnum>()
        where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .ToArray();
    }

    public static IDictionary<string, TEnum> GetDescriptions<TEnum>()
        where TEnum : Enum
    {
        if (!typeof(TEnum).IsEnum)
        {
            throw new ArgumentException("Type is not an enumerator.");
        }

        return Enum.GetValues(typeof(TEnum))
            .OfType<TEnum>()
            .ToDictionary(x => x.GetDescription()!, x => x);
    }

    public static TEnum ParseEnum<TEnum>(string input, bool ignorecCase = false)
        where TEnum : Enum
    {
        var enumParseObject = Enum.Parse(typeof(TEnum), input, ignorecCase);

        return (TEnum)enumParseObject;
    }
}
