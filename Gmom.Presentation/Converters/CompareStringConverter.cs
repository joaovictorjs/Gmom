using System.Globalization;
using System.Windows.Data;

namespace Gmom.Presentation.Converters;

public class CompareStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string stringValue || parameter is not string stringParameter) return false;

        return stringValue == stringParameter;
    }

    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        if (value is not string stringValue || parameter is not string stringParameter) return false;

        return stringValue != stringParameter;
    }
}