using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using Gmom.Domain.Enums;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;
using Gmom.Infrastructure.Exceptions;
using Gmom.Presentation.Views;

namespace Gmom.Presentation.ViewModels;

public class LocateProductsViewModel : BindableBase
{
    private readonly IWindowService<
        InsertOrUpdateProductView,
        InsertOrUpdateProductViewModel
    > _productUpdateWindowService;
    private readonly DispatcherTimer _dispatcherTimer;
    private readonly IProductService _productService;
    private readonly IMessageBoxService _messageBoxService;

    private string _searchTerm = string.Empty;
    private string _findStrategyName = FindStrategy.Name.ToString();

    public string SearchTerm
    {
        get => _searchTerm.ToUpper();
        set
        {
            SetProperty(ref _searchTerm, value.ToUpper());
            ResetTimer();
        }
    }

    public string FindStrategyName
    {
        get => _findStrategyName;
        set
        {
            SetProperty(ref _findStrategyName, value);
            ResetTimer();
        }
    }

    public ObservableCollection<ProductModel> Products { get; } = [];

    public DelegateCommand InsertProductCommand { get; }
    public DelegateCommand<string> ChangeFindStrategyCommand { get; }

    public LocateProductsViewModel(
        IWindowService<
            InsertOrUpdateProductView,
            InsertOrUpdateProductViewModel
        > productUpdateWindowService, IProductService productService, IMessageBoxService messageBoxService)
    {
        _productUpdateWindowService = productUpdateWindowService;
        _productService = productService;
        _messageBoxService = messageBoxService;

        _dispatcherTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };

        _dispatcherTimer.Tick += async (_, _) =>
        {
            _dispatcherTimer.Stop();

            await SearchProducts();
        };

        InsertProductCommand = new DelegateCommand(InsertProduct);
        ChangeFindStrategyCommand = new DelegateCommand<string>(ChangeFindStrategy);
    }

    private void ChangeFindStrategy(string strategy)
    {
        FindStrategyName = strategy;
    }

    private void ResetTimer()
    {
        _dispatcherTimer.Stop();
        _dispatcherTimer.Start();
    }

    private async Task SearchProducts()
    {
        try
        {
            var data = await _productService.Find(_searchTerm, Enum.Parse<FindStrategy>(_findStrategyName));
            Products.Clear();
            Products.AddRange(data);
        }
        catch (Exception ex)
        {
            ex.UseGlobalHandle(_messageBoxService.ShowError);
        }
    }

    private void InsertProduct()
    {
        _productUpdateWindowService.Create().ShowDialog();
    }
}
