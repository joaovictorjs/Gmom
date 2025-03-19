using Gmom.Domain.Interface;
using Gmom.Domain.Models;
using Gmom.Infrastructure.Exceptions;

namespace Gmom.Presentation.ViewModels;

public class InsertOrUpdateCustomerViewModel : BindableBase, IClosableWindow
{
    private readonly ICustomerService _customerService;
    private readonly IMessageBoxService _messageBoxService;
    public Action? Close { get; set; }


    private int _id;
    private string _name = string.Empty;
    private bool _isEditMode = false;

    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public AsyncDelegateCommand SaveCommand { get; }

    public InsertOrUpdateCustomerViewModel(
        ICustomerService customerService,
        IMessageBoxService messageBoxService,
        CustomerModel? customer = null
    )
    {
        _customerService = customerService;
        _messageBoxService = messageBoxService;

        
        if (customer != null)
        {
            _id = customer.Id;
            _name = customer.Name;
            _isEditMode = true;
        }
        else
        {
            _id = _customerService.GetNextId();
        }
        
        SaveCommand = new AsyncDelegateCommand(Save, CanSave);
    }

    private bool CanSave()
    {
        return Id > 0 && !string.IsNullOrWhiteSpace(Name);
    }

    private async Task Save()
    {
        try
        {
            await _customerService.Save(new CustomerModel { Id = Id, Name = Name }, _isEditMode);
            
            _messageBoxService.ShowInformation("Cliente salvo com sucesso!");
            
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
}
