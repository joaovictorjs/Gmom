using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using System.Windows.Threading;
using Gmom.Domain.Enums;
using Gmom.Domain.Extensions;
using Gmom.Domain.Interface;
using Gmom.Infrastructure.Exceptions;

namespace Gmom.Presentation.ViewModels;

public class ProductPanelViewModel : BindableBase
{
    private readonly DispatcherTimer _timer;
    private readonly IMessageBoxService _messageBoxService;
    private readonly IProductService _productService;

    public ProductPanelViewModel(
        IMessageBoxService messageBoxService,
        IProductService productService
    )
    {
        _messageBoxService = messageBoxService;
        _productService = productService;
        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };

        _timer.Tick += async (_, _) =>
        {
            _timer.Stop();

            await SearchProduct();
        };
    }

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

            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        _timer.Stop();
        _timer.Start();
    }

    private static string RemoveInvalidChars(string value)
    {
        var chars = value
            .Where(it =>
                char.IsNumber(it) || it == '*' || it == ',' || it == '.' || char.IsWhiteSpace(it)
            )
            .ToArray();

        return new string(chars);
    }

    private void SplitQuantityAndProduct(string value)
    {
        if (value.IsNullOrWhiteSpace())
            return;

        var values = value.Split('*', 2).Select(it => it.Trim()).ToList();

        _quantity =
            values.Count == 1 ? 1
            : double.TryParse(values[0], CultureInfo.CurrentCulture, out var number) ? number
            : 1;
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

    private string _total = string.Empty;
    public string Total
    {
        get => _total;
        set => SetProperty(ref _total, value);
    }

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private async Task SearchProduct()
    {
        try
        {
            var result = (
                await _productService.Find(
                    _idBarCode,
                    _idBarCode.Length >= 13 ? FindStrategy.BarCode : FindStrategy.Id
                )
            ).FirstOrDefault();

            if (result == null)
            {
                Name = string.Empty;
                Price = string.Empty;
                Discount = string.Empty;
                Total = string.Empty;
            }
            else
            {
                Name = result.Name;
                Price = result.Price.ToString(CultureInfo.CurrentCulture);
                Discount = string.Empty;
                Total = (result.Price * _quantity).ToString("#.##",CultureInfo.CurrentCulture);
            }
        }
        catch (Exception ex)
        {
            ex.UseGlobalHandle(_messageBoxService.ShowError);
        }
    }
}
