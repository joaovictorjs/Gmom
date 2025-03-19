using Gmom.Presentation.Events;

namespace Gmom.Presentation.ViewModels;

public class CustomerPanelViewModel : BindableBase
{
    private readonly IEventAggregator _eventAggregator;
    private string _customerName = string.Empty;

    public string CustomerName
    {
        get => _customerName;
        set => SetProperty(ref _customerName, value);
    }

    public CustomerPanelViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;

        _eventAggregator
            .GetEvent<CustomerSelected>()
            .Subscribe(customer => CustomerName = customer.Name);
    }
}
