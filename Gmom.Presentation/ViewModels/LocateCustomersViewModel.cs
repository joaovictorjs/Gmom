using Gmom.Domain.Interface;
using Gmom.Presentation.Views;

namespace Gmom.Presentation.ViewModels;

public class LocateCustomersViewModel : BindableBase
{
    private readonly IWindowService<
        InsertOrUpdateCustomerView,
        InsertOrUpdateCustomerViewModel
    > _customerWindowService;

    public DelegateCommand InsertCustomerCommand { get; }

    public LocateCustomersViewModel(
        IWindowService<
            InsertOrUpdateCustomerView,
            InsertOrUpdateCustomerViewModel
        > customerWindowService
    )
    {
        _customerWindowService = customerWindowService;

        InsertCustomerCommand = new DelegateCommand(InsertCustomer);
    }

    private void InsertCustomer()
    {
        _customerWindowService.Create().ShowDialog();
    }
}
