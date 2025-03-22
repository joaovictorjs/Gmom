using System.Globalization;
using System.Text.RegularExpressions;
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
    private readonly IEventAggregator _eventAggregator;
    private readonly DispatcherTimer _timer;
    private readonly IProductService _productService;
    private readonly IMessageBoxService _messageBoxService;

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string _price = string.Empty;
    public string Price
    {
        get => _price;
        set
        {
            SetProperty(ref _price, value.ToBrazilianReal(_price));
            UpdateTotalPrice();
        }
    }

    private int _quantity = 1;
    private string _productCode = string.Empty;
    private string _productVersusQuantity = string.Empty;
    public string ProductVersusQuantity
    {
        get => _productVersusQuantity;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                SetProperty(ref _productVersusQuantity, string.Empty);
                _productCode = string.Empty;
                _quantity = 1;
                TimerReset();
                return;
            }

            if (!Regex.IsMatch(value, @"^[0-9 *]+$"))
                return;

            var elements = value
                .Split('*', 2)
                .Select(it => it.Replace(" ", string.Empty))
                .Where(it => string.IsNullOrWhiteSpace(it) == false)
                .ToArray();

            if (elements.Length == 2)
            {
                if (int.TryParse(elements[0], out var parsedQuantity) && elements[1].Length <= 13)
                {
                    _quantity = parsedQuantity;
                    _productCode = elements[1];
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (elements[0].Length <= 13)
                {
                    _quantity = 1;
                    _productCode = elements[0];
                }
                else
                {
                    return;
                }
            }

            SetProperty(ref _productVersusQuantity, value);
            TimerReset();
        }
    }

    private void TimerReset()
    {
        _timer.Stop();
        _timer.Start();
    }

    private string _discount = string.Empty;
    public string Discount
    {
        get => _discount;
        set
        {
            SetProperty(ref _discount, value.ToDiscount(_discount));
            UpdateTotalPrice();
        }
    }

    private void UpdateTotalPrice()
    {
        if (string.IsNullOrWhiteSpace(_price))
        {
            TotalPrice = string.Empty;
            return;
        }

        var discountRate = 1.0;
        if (!string.IsNullOrWhiteSpace(_discount))
        {
            discountRate = (100 - _discount.FromDiscountToDouble()) / 100;
        }

        var totalProduct = _price.FromBrazilianRealToDouble() * _quantity;
        TotalPrice = string.Format(new CultureInfo("pt-BR"), "{0:C}", totalProduct * discountRate);
    }

    private string _totalPrice = string.Empty;
    public string TotalPrice
    {
        get => _totalPrice;
        set => SetProperty(ref _totalPrice, value);
    }

    public ProductPanelViewModel(
        IEventAggregator eventAggregator,
        IProductService productService,
        IMessageBoxService messageBoxService
    )
    {
        _eventAggregator = eventAggregator;
        _productService = productService;
        _messageBoxService = messageBoxService;
        _eventAggregator.GetEvent<ProductSelected>().Subscribe(OnProductSelected);

        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };

        _timer.Tick += async (_, _) =>
        {
            _timer.Stop();

            await SearchProduct();
        };
    }

    private void OnProductSelected(ProductModel product)
    {
        Name = product.Name;
        Price = product.Price.ToString(CultureInfo.CurrentCulture);
        Discount = "0";
        ProductVersusQuantity = $"1 * {product.BarCode}";
        _timer.Stop();
    }

    private async Task SearchProduct()
    {
        try
        {
            var product = await _productService.FindOne(
                _productCode,
                _productCode.Length == 13 ? FindStrategy.BarCode : FindStrategy.Id
            );

            if (product != null)
            {
                Name = product.Name;
                Price = product.Price.ToString(CultureInfo.CurrentCulture);
                Discount = "0";
            }
            else
            {
                Name = string.Empty;
                Price = string.Empty;
                Discount = string.Empty;
            }

            _timer.Stop();
        }
        catch (Exception ex)
        {
            ex.UseGlobalHandle(_messageBoxService.ShowError);
        }
    }
}
