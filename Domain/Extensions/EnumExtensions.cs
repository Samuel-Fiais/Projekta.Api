using System.ComponentModel;

namespace Domain.Extensions;

public static class EnumExtensions
{
    public static string GetValue(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        return
            field?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault()
                is DescriptionAttribute attribute
            ? attribute.Description
            : value.ToString();
    }
}
