using System.Collections.ObjectModel;
using System.Windows;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;
using Gmom.Infrastructure.Exceptions;

namespace Gmom.Presentation.ViewModels;

public class ManageUsersViewModel : BindableBase
{
    private IUserService _userService;
    private IMessageBoxService _messageBoxService;

    private bool _isReadMode = true;
    private int _selectedIndex = -1;
    private int _id;
    private bool _isAdmin;
    private string _name = string.Empty;
    private string _password = string.Empty;
    private string _confirmPassword = string.Empty;

    public bool IsReadMode
    {
        get => _isReadMode;
        set => SetProperty(ref _isReadMode, value);
    }

    public bool IsEditMode => !IsReadMode;

    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }
    public bool IsAdmin
    {
        get => _isAdmin;
        set => SetProperty(ref _isAdmin, value);
    }
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set => SetProperty(ref _confirmPassword, value);
    }

    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            SetProperty(ref _selectedIndex, value);

            if (value < 0)
                return;

            Id = Users[value].Id;
            IsAdmin = Users[value].IsAdmin;
            Name = Users[value].Name;
        }
    }

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
        try
        {
            Users.Clear();
            Users.AddRange(await _userService.GetAll());
        }
        catch (Exception ex)
        {
            ex.UseGlobalHandle(_messageBoxService.ShowError);
        }
    }

    protected override bool SetProperty<T>(ref T storage, T value, string? propertyName = null)
    {
        InsertUserCommand.RaiseCanExecuteChanged();
        CancelCommand.RaiseCanExecuteChanged();
        return base.SetProperty(ref storage, value, propertyName);
    }
}
