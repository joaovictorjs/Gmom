using System.Collections.ObjectModel;
using System.Windows.Threading;
using Gmom.Domain.Enums;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;
using Gmom.Presentation.Events;
using Gmom.Presentation.Views;

namespace Gmom.Presentation.ViewModels;

public class LocateCustomersViewModel : BindableBase
{
    private readonly IWindowService<
        InsertOrUpdateCustomerView,
        InsertOrUpdateCustomerViewModel
    > _customerWindowService;
    private readonly ICustomerService _customerService;

    private string _searchTerm = string.Empty;
    private string _findStrategyName = FindStrategy.Name.ToString();
    private readonly DispatcherTimer _timer;

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

    public ObservableCollection<CustomerModel> Customers { get; } = [];

    public DelegateCommand<string> ChangeFindStrategyCommand { get; }
    public DelegateCommand InsertCustomerCommand { get; }

    public LocateCustomersViewModel(
        IWindowService<
            InsertOrUpdateCustomerView,
            InsertOrUpdateCustomerViewModel
        > customerWindowService,
        ICustomerService customerService
    )
    {
        _customerWindowService = customerWindowService;
        _customerService = customerService;

        ChangeFindStrategyCommand = new DelegateCommand<string>(ChangeFindStrategy);
        InsertCustomerCommand = new DelegateCommand(InsertCustomer);

        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };

        _timer.Tick += async (_, _) =>
        {
            _timer.Stop();

            await SearchCustomers();
        };
    }

    private void ResetTimer()
    {
        _timer.Stop();
        _timer.Start();
    }

    private async Task SearchCustomers()
    {
        Customers.Clear();

        Customers.AddRange(
            await _customerService.Find(SearchTerm, Enum.Parse<FindStrategy>(FindStrategyName))
        );
    }

    private void ChangeFindStrategy(string strategy)
    {
        FindStrategyName = strategy;
    }

    private void InsertCustomer()
    {
        _customerWindowService.Create().ShowDialog();
    }
}
