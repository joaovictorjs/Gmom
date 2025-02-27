using System.Globalization;
using System.Windows.Data;

namespace Gmom.Presentation.Converters;

public class PercentageToDimensionConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(parameter);

        return double.Parse(parameter.ToString()!) / 100 * double.Parse(value.ToString()!);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}