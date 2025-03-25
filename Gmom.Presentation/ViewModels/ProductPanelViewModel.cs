using System.Globalization;
using Gmom.Domain.Extensions;

namespace Gmom.Presentation.ViewModels;

public class ProductPanelViewModel : BindableBase
{
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
