using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using Gmom.Domain.Extensions;

namespace Gmom.Presentation.ViewModels;

public class ProductPanelViewModel : BindableBase
{
    private double _quantity = 1.0;
    private string _idBarCode = string.Empty;

    private string _quantityVersusProduct = string.Empty;
    public string QuantityVersusProduct
    {
        get => _quantityVersusProduct;
        set
        {
            SetProperty(ref _quantityVersusProduct, RemoveInvalidChars(value));

            SplitQuantityAndProduct(value);
        }
    }

    private static string RemoveInvalidChars(string value)
    {
        var chars = value
            .Where(it => char.IsNumber(it) || it == '*' || char.IsWhiteSpace(it))
            .ToArray();

        return new string(chars);
    }

    private void SplitQuantityAndProduct(string value)
    {
        if (value.IsNullOrWhiteSpace())
            return;

        var values = value.Split('*', 2).Select(it => it.Trim()).ToList();

        _quantity = values.Count == 1 ? 1 : double.Parse(values[0], CultureInfo.CurrentCulture);
        _idBarCode = values.Count == 1 ? values[0] : values[1];
    }

    private string _price = string.Empty;
    public string Price
    {
        get => _price;
        set
        {
            if (value.IsNullOrWhiteSpace())
            {
                SetProperty(ref _price, string.Empty);
                return;
            }

            if (
                !double.TryParse(
                    value,
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out var number
                )
                || number < 0
            )
                return;

            SetProperty(ref _price, value);
        }
    }

    private string _discount = string.Empty;
    public string Discount
    {
        get => _discount;
        set
        {
            if (value.IsNullOrWhiteSpace())
            {
                SetProperty(ref _discount, string.Empty);
                return;
            }

            if (
                !double.TryParse(
                    value,
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out var number
                )
                || number < 0
                || number > 100
            )
                return;

            SetProperty(ref _discount, value);
        }
    }
}
