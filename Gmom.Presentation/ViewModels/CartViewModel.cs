using System.Collections.ObjectModel;
using Gmom.Domain.Models;
using Gmom.Presentation.Events;

namespace Gmom.Presentation.ViewModels;

public class CartViewModel : BindableBase
{
    private readonly IEventAggregator _eventAggregator;

    public ObservableCollection<CartProductModel> CartProducts { get; } = [];

    private int _selectedIndex = -1;
    public int SelectedIndex
    {
        get => _selectedIndex;
        set => SetProperty(ref _selectedIndex, value);
    }
    
    public CartViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;

        _eventAggregator.GetEvent<CartProductAdded>().Subscribe(OnProductCardAdded);
    }

    private void OnProductCardAdded(CartProductModel cartProduct)
    {
        CartProducts.Add(cartProduct);
    }

    protected override bool SetProperty<T>(ref T storage, T value, string? propertyName = null)
    {
        return base.SetProperty(ref storage, value, propertyName);
    }
}
