using Gmom.Domain.Enums;
using Gmom.Domain.Interface;
using Gmom.Presentation.Events;
using Gmom.Presentation.Views;

namespace Gmom.Presentation.ViewModels;

public class LocateCustomersViewModel : BindableBase
{
    private readonly IWindowService<
        InsertOrUpdateCustomerView,
        InsertOrUpdateCustomerViewModel
    > _customerWindowService;

    private string _findStrategyName = "Name";

    public string FindStrategyName
    {
        get => _findStrategyName;
        set => SetProperty(ref _findStrategyName, value);
    }

    public DelegateCommand<string> ChangeFindStrategyCommand { get; }
    public DelegateCommand InsertCustomerCommand { get; }

    public LocateCustomersViewModel(
        IWindowService<
            InsertOrUpdateCustomerView,
            InsertOrUpdateCustomerViewModel
        > customerWindowService
    )
    {
        _customerWindowService = customerWindowService;

        ChangeFindStrategyCommand = new DelegateCommand<string>(ChangeFindStrategy);
        InsertCustomerCommand = new DelegateCommand(InsertCustomer);
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
