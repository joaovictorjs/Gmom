using System.Globalization;

namespace Gmom.Domain.Extensions;

public static class StringExtensions
{
    public static string ToBrazilianReal(this string str, string fallback)
    {
        if (string.IsNullOrWhiteSpace(str) || str.Trim().Equals("R$"))
        {
            return string.Empty;
        }

        var number = str.Replace("R$", string.Empty).Trim();

        return !double.TryParse(number, NumberStyles.Currency, CultureInfo.CurrentCulture, out _)
            ? fallback
            : $"R$ {number}";
    }
}
