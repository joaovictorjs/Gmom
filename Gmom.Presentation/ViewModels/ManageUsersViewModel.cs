using System.Collections.ObjectModel;
using System.Windows;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;

namespace Gmom.Presentation.ViewModels;

public class ManageUsersViewModel : BindableBase
{
    private IUserService _userService;
    private IMessageBoxService _messageBoxService;

    private bool _isReadMode = true;

    public bool IsReadMode
    {
        get => _isReadMode;
        set => SetProperty(ref _isReadMode, value);
    }
    
    public bool IsEditMode => !IsReadMode;

    public ObservableCollection<UserModel> Users { get; } = [];
    
    public DelegateCommand InsertUserCommand { get; }

    public DelegateCommand CancelCommand { get; }
    
    public ManageUsersViewModel(IUserService userService, IMessageBoxService messageBoxService)
    {
        _userService = userService;
        _messageBoxService = messageBoxService;

        _ = LoadUsers();

        InsertUserCommand = new DelegateCommand(InsertUser, CanInsertUser);
        CancelCommand = new DelegateCommand(Cancel, CanCancel);
    }

    private bool CanCancel()
    {
        return IsEditMode;
    }

    private void Cancel()
    {
        IsReadMode = true;
    }

    private bool CanInsertUser()
    {
        return IsReadMode;
    }

    private void InsertUser()
    {
        IsReadMode = false;
        
        // todo: generate next user id
    }

    private async Task LoadUsers()
    {
        Users.Clear();
        Users.AddRange(await _userService.GetAll());
    }

    protected override bool SetProperty<T>(ref T storage, T value, string? propertyName = null)
    {
        InsertUserCommand.RaiseCanExecuteChanged();
        CancelCommand.RaiseCanExecuteChanged();
        return base.SetProperty(ref storage, value, propertyName);
    }
}
