using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using System.Windows.Threading;
using Gmom.Domain.Enums;
using Gmom.Domain.Extensions;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;
using Gmom.Infrastructure.Exceptions;
using Gmom.Presentation.Events;

namespace Gmom.Presentation.ViewModels;

public class ProductPanelViewModel : BindableBase
{
    private readonly DispatcherTimer _timer;
    private readonly IMessageBoxService _messageBoxService;
    private readonly IProductService _productService;
    private readonly IEventAggregator _eventAggregator;
    private ProductModel? _actualProduct;

    public DelegateCommand AddProductToCartCommand { get; }

    public ProductPanelViewModel(
        IMessageBoxService messageBoxService,
        IProductService productService,
        IEventAggregator eventAggregator
    )
    {
        _messageBoxService = messageBoxService;
        _productService = productService;
        _eventAggregator = eventAggregator;
        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };

        _timer.Tick += async (_, _) =>
        {
            _timer.Stop();

            await SearchProduct();
        };

        AddProductToCartCommand = new DelegateCommand(AddProductToCart, CanAddProductToCart);
    }

    private bool CanAddProductToCart()
    {
        return _actualProduct != null
            && double.TryParse(_price, out _)
            && double.TryParse(_total, out _);
    }

    private void AddProductToCart()
    {
        _eventAggregator
            .GetEvent<CartProductAdded>()
            .Publish(
                new CartProductModel
                {
                    Id = _actualProduct!.Id,
                    Name = _name,
                    BarCode = _actualProduct!.BarCode,
                    Price = double.Parse(_price),
                    Total = double.Parse(_total),
                    Quantity = _quantity,
                }
            );
        
        Name = string.Empty;
        QuantityVersusProduct = string.Empty;
        Price = string.Empty;
        Discount = string.Empty;
        Total = string.Empty;
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
        {
            _quantity = 0.0;
            _idBarCode = string.Empty;
            return;
        }

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
                CalculateTotal();
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

            CalculateTotal();
        }
    }

    private void CalculateTotal()
    {
        var price = _price.IsNullOrWhiteSpace()
            ? 0
            : double.Parse(_price, CultureInfo.CurrentCulture);
        var discount = _discount.IsNullOrWhiteSpace()
            ? 0
            : double.Parse(_discount, CultureInfo.CurrentCulture) / 100;
        var totalPrice = price * _quantity;
        Total = (totalPrice - (totalPrice * discount)).ToString(
            "0#.##",
            CultureInfo.CurrentCulture
        );
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
                CalculateTotal();
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

            CalculateTotal();
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
            _actualProduct = (
                await _productService.Find(
                    _idBarCode,
                    _idBarCode.Length >= 13 ? FindStrategy.BarCode : FindStrategy.Id
                )
            ).FirstOrDefault();

            if (_actualProduct == null)
            {
                Name = string.Empty;
                Price = string.Empty;
                Discount = string.Empty;
                Total = string.Empty;
            }
            else
            {
                Name = _actualProduct.Name;
                Price = _actualProduct.Price.ToString(CultureInfo.CurrentCulture);
                Discount = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ex.UseGlobalHandle(_messageBoxService.ShowError);
        }
    }
}
