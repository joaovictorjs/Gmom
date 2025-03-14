using System.Globalization;
using System.Windows.Data;

namespace Gmom.Presentation.Converters;

public class InverseBooleanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool boolValue)
        {
            throw new ArgumentException("Value must be of type bool");
        }
        
        return !boolValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}