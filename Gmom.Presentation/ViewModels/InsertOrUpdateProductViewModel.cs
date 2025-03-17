using System.Globalization;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;
using Gmom.Infrastructure.Exceptions;

namespace Gmom.Presentation.ViewModels;

public class InsertOrUpdateProductViewModel : BindableBase, IClosableWindow
{
    private readonly IProductService _productService;
    private readonly IMessageBoxService _messageBoxService;

    private int _id;
    private string _name = string.Empty;
    private string _barCode = string.Empty;
    private double _price;
    private double _stock;
    private readonly bool _isEditMode;

    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }
    public string Name
    {
        get => _name.ToUpper();
        set => SetProperty(ref _name, value.ToUpper());
    }
    public string BarCode
    {
        get => _barCode;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                SetProperty(ref _barCode, value);
                return;
            }

            if (value.Length > 13)
            {
                return;
            }
            
            if (ulong.TryParse(value, out var result))
            {
                SetProperty(ref _barCode, result.ToString());
            }
        }
    }
    public string Price
    {
        get => _price.ToString("C");
        set
        {            
            if (string.IsNullOrWhiteSpace(value)) return;

            if (
                !double.TryParse(
                    value,
                    NumberStyles.Currency,
                    CultureInfo.CurrentCulture,
                    out var price
                )
            )
                return;

            SetProperty(ref _price, price);
        }
    }
    public string Stock
    {
        get => _stock.ToString("N");
        set
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            
            if (
                !double.TryParse(
                    value,
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out var stock
                )
            )
                return;

            SetProperty(ref _stock, stock);
        }
    }

    public AsyncDelegateCommand SaveCommand { get; }
    public DelegateCommand GenerateBarCodeCommand { get; }

    public InsertOrUpdateProductViewModel(
        IProductService productService,
        IMessageBoxService messageBoxService,
        ProductModel? product = null
    )
    {
        _productService = productService;
        _messageBoxService = messageBoxService;

        if (product != null)
        {
            _id = product.Id;
            _name = product.Name;
            _barCode = product.BarCode;
            _price = product.Price;
            _stock = product.Stock;
            _isEditMode = true;
        }
        else
        {
            _id = productService.GetNextId();
        }

        SaveCommand = new AsyncDelegateCommand(Save, CanSave);
        GenerateBarCodeCommand = new DelegateCommand(GenerateBarCode);
    }

    private void GenerateBarCode()
    {
        BarCode = _productService.GenerateBarCode();
    }

    private bool CanSave()
    {
        return Id > 0 && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(BarCode);
    }

    private async Task Save()
    {
        try
        {
            await _productService.Save(
                new ProductModel
                {
                    Id = _id,
                    Name = _name,
                    BarCode = _barCode,
                    Price = _price,
                    Stock = _stock,
                },
                _isEditMode
            );
            
            _messageBoxService.ShowInformation($"Produto salvo com sucesso!");
            Close?.Invoke();
        }
        catch (Exception ex)
        {
            ex.UseGlobalHandle(_messageBoxService.ShowError);
        }
    }

    protected override bool SetProperty<T>(ref T storage, T value, string? propertyName = null)
    {
        SaveCommand.RaiseCanExecuteChanged();
        return base.SetProperty(ref storage, value, propertyName);
    }

    public Action? Close { get; set; }
}
