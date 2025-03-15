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
    private bool _isEditMode;
    private bool _isInsertMode;

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

    public bool IsEditMode
    {
        get => _isEditMode;
        set => SetProperty(ref _isEditMode, value);
    }

    public bool IsInsertMode
    {
        get => _isInsertMode;
        set => SetProperty(ref _isInsertMode, value);
    }

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
        get => _name.ToUpper();
        set => SetProperty(ref _name, value.ToUpper());
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
            {
                CleanupUserInformation();
                return;
            }

            Id = Users[value].Id;
            IsAdmin = Users[value].IsAdmin;
            Name = Users[value].Name;
        }
    }

    public ObservableCollection<UserModel> Users { get; } = [];

    public DelegateCommand InsertUserCommand { get; }
    public DelegateCommand EditUserCommand { get; }
    public AsyncDelegateCommand DeleteUserCommand { get; }
    public DelegateCommand CancelCommand { get; }
    public AsyncDelegateCommand SaveUserCommand { get; }

    public ManageUsersViewModel(IUserService userService, IMessageBoxService messageBoxService)
    {
        _userService = userService;
        _messageBoxService = messageBoxService;

        _ = LoadUsers();

        InsertUserCommand = new DelegateCommand(InsertUser, CanInsertUser);
        EditUserCommand = new DelegateCommand(EditUser, CanEditUser);
        CancelCommand = new DelegateCommand(Cancel, CanCancel);
        DeleteUserCommand = new AsyncDelegateCommand(Delete, CanDelete);
        SaveUserCommand = new AsyncDelegateCommand(SaveUser, CanSaveUser);
    }

    private bool CanDelete()
    {
        return IsReadMode && !IsEditMode && !IsInsertMode && SelectedIndex > -1;
    }

    private async Task Delete()
    {
        try
        {
            var res = _messageBoxService.ShowConfirmation(
                "Tem certeza que deseja deletar esse usuário?"
            );

            if (res == false)
                return;

            await _userService.Delete(Users[SelectedIndex]);

            _messageBoxService.ShowInformation("Usuário deletado!");

            await LoadUsers();

            SelectedIndex = -1;
        }
        catch (Exception e)
        {
            e.UseGlobalHandle(_messageBoxService.ShowError);
        }
    }

    private bool CanEditUser()
    {
        return IsReadMode && !IsEditMode && !IsInsertMode && SelectedIndex > -1;
    }

    private void EditUser()
    {
        IsReadMode = false;
        IsEditMode = true;
        IsInsertMode = false;
    }

    private bool CanSaveUser()
    {
        return !IsReadMode && !string.IsNullOrWhiteSpace(Name) && Password.Equals(ConfirmPassword);
    }

    private async Task SaveUser()
    {
        try
        {
            await _userService.Save(
                new UserModel
                {
                    Id = Id,
                    Name = Name,
                    Password = ConfirmPassword,
                    IsAdmin = IsAdmin,
                },
                IsEditMode ? Users[_selectedIndex] : null
            );

            _messageBoxService.ShowInformation("Usuário salvo com sucesso!");

            await LoadUsers();

            SelectedIndex = -1;
            IsReadMode = true;
            IsEditMode = false;
            IsInsertMode = false;
        }
        catch (Exception e)
        {
            e.UseGlobalHandle(_messageBoxService.ShowError);
        }
    }

    private bool CanCancel()
    {
        return !IsReadMode;
    }

    private void Cancel()
    {
        IsReadMode = true;
        IsEditMode = false;
        IsInsertMode = false;
        SelectedIndex = -1;
    }

    private bool CanInsertUser()
    {
        return IsReadMode && !IsEditMode && !IsInsertMode;
    }

    private void InsertUser()
    {
        CleanupUserInformation();

        IsReadMode = false;
        IsEditMode = false;
        IsInsertMode = true;

        Id = _userService.GetNextId();
    }

    private void CleanupUserInformation()
    {
        Id = 0;
        IsAdmin = false;
        Name = string.Empty;
        Password = string.Empty;
        ConfirmPassword = string.Empty;
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
        EditUserCommand.RaiseCanExecuteChanged();
        CancelCommand.RaiseCanExecuteChanged();
        SaveUserCommand.RaiseCanExecuteChanged();
        return base.SetProperty(ref storage, value, propertyName);
    }
}
