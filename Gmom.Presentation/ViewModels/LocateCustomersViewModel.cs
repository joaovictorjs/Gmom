using System.Collections.ObjectModel;
using System.Windows.Threading;
using Gmom.Domain.Enums;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;
using Gmom.Infrastructure.Exceptions;
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
    private readonly IMessageBoxService _messageBoxService;

    private string _searchTerm = string.Empty;
    private string _findStrategyName = FindStrategy.Name.ToString();
    private int _selectedIndex = -1;
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

    public int SelectedIndex
    {
        get => _selectedIndex;
        set => SetProperty(ref _selectedIndex, value);
    }

    public ObservableCollection<CustomerModel> Customers { get; } = [];

    public DelegateCommand<string> ChangeFindStrategyCommand { get; }
    public DelegateCommand InsertCustomerCommand { get; }
    public AsyncDelegateCommand DeleteCustomerCommand { get; }
    public AsyncDelegateCommand EditCustomerCommand { get; }

    public LocateCustomersViewModel(
        IWindowService<
            InsertOrUpdateCustomerView,
            InsertOrUpdateCustomerViewModel
        > customerWindowService,
        ICustomerService customerService,
        IMessageBoxService messageBoxService
    )
    {
        _customerWindowService = customerWindowService;
        _customerService = customerService;
        _messageBoxService = messageBoxService;

        ChangeFindStrategyCommand = new DelegateCommand<string>(ChangeFindStrategy);
        InsertCustomerCommand = new DelegateCommand(InsertCustomer);
        DeleteCustomerCommand = new AsyncDelegateCommand(DeleteCustomer, CanDeleteCustomer);
        EditCustomerCommand = new AsyncDelegateCommand(EditCustomer, CanEditCustomer);

        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };

        _timer.Tick += async (_, _) =>
        {
            _timer.Stop();

            await SearchCustomers();
        };
    }

    private bool CanEditCustomer()
    {
        return _selectedIndex > -1;
    }

    private async Task EditCustomer()
    {
        try
        {
            _customerWindowService
                .Create(new KeyValuePair<string, object?>("customer", Customers[_selectedIndex]))
                .ShowDialog();
            
            await SearchCustomers();
        }
        catch (Exception ex)
        {
            ex.UseGlobalHandle(_messageBoxService.ShowError);
        }
    }

    private bool CanDeleteCustomer()
    {
        return _selectedIndex > -1;
    }

    private async Task DeleteCustomer()
    {
        try
        {
            var resp = _messageBoxService.ShowConfirmation(
                "Tem certeza que deseja deletar esse cliente?"
            );

            if (!resp)
                return;

            await _customerService.Delete(Customers[_selectedIndex]);

            await SearchCustomers();

            _messageBoxService.ShowInformation("Cliente deletado!");
        }
        catch (Exception ex)
        {
            ex.UseGlobalHandle(_messageBoxService.ShowError);
        }
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

    protected override bool SetProperty<T>(ref T storage, T value, string? propertyName = null)
    {
        DeleteCustomerCommand.RaiseCanExecuteChanged();
        EditCustomerCommand.RaiseCanExecuteChanged();
        return base.SetProperty(ref storage, value, propertyName);
    }
}
