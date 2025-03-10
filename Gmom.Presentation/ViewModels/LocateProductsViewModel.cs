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
    private int _selectedIndex = -1;

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

    public int SelectedIndex
    {
        get => _selectedIndex;
        set => SetProperty(ref _selectedIndex, value);
    }

    public ObservableCollection<ProductModel> Products { get; } = [];

    public AsyncDelegateCommand InsertProductCommand { get; }
    public AsyncDelegateCommand EditProductCommand { get; }
    public AsyncDelegateCommand DeleteProductCommand { get; }
    public DelegateCommand<string> ChangeFindStrategyCommand { get; }

    public LocateProductsViewModel(
        IWindowService<
            InsertOrUpdateProductView,
            InsertOrUpdateProductViewModel
        > productUpdateWindowService,
        IProductService productService,
        IMessageBoxService messageBoxService
    )
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

        InsertProductCommand = new AsyncDelegateCommand(InsertProduct);
        EditProductCommand = new AsyncDelegateCommand(EditProduct, CanEditProduct);
        DeleteProductCommand = new AsyncDelegateCommand(DeleteProduct, CanDeleteProduct);
        ChangeFindStrategyCommand = new DelegateCommand<string>(ChangeFindStrategy);
    }

    private bool CanEditProduct()
    {
        return SelectedIndex > -1;
    }

    private bool CanDeleteProduct()
    {
        return SelectedIndex > -1;
    }

    private async Task DeleteProduct()
    {
        try
        {
            var res = _messageBoxService.ShowConfirmation(
                "Tem certeza que deseja deletar esse produto?"
            );

            if (res == false)
                return;

            await _productService.Delete(Products[SelectedIndex]);

            await SearchProducts();

            _messageBoxService.ShowInformation("Produto deletado!");
        }
        catch (Exception ex)
        {
            ex.UseGlobalHandle(_messageBoxService.ShowError);
        }
    }

    private async Task EditProduct()
    {
        _productUpdateWindowService
            .Create(new KeyValuePair<string, object?>("product", Products[SelectedIndex]))
            .ShowDialog();
        
        await SearchProducts();
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
            var data = await _productService.Find(
                _searchTerm,
                Enum.Parse<FindStrategy>(_findStrategyName)
            );
            Products.Clear();
            Products.AddRange(data);
        }
        catch (Exception ex)
        {
            ex.UseGlobalHandle(_messageBoxService.ShowError);
        }
    }

    private async Task InsertProduct()
    {
        _productUpdateWindowService.Create().ShowDialog();
        await SearchProducts();
    }

    protected override bool SetProperty<T>(ref T storage, T value, string? propertyName = null)
    {
        DeleteProductCommand.RaiseCanExecuteChanged();
        EditProductCommand.RaiseCanExecuteChanged();
        return base.SetProperty(ref storage, value, propertyName);
    }
}
