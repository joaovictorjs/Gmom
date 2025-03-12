using System.Collections.ObjectModel;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;

namespace Gmom.Presentation.ViewModels;

public class ManageUsersViewModel : BindableBase
{
    private IUserService _userService;
    private IMessageBoxService _messageBoxService;

    private bool _isEditMode;

    public bool IsEditMode
    {
        get => _isEditMode;
        set => SetProperty(ref _isEditMode, value);
    }

    public ObservableCollection<UserModel> Users { get; } = [];

    public ManageUsersViewModel(IUserService userService, IMessageBoxService messageBoxService)
    {
        _userService = userService;
        _messageBoxService = messageBoxService;

        _ = LoadUsers();
    }

    private async Task LoadUsers()
    {
        Users.Clear();
        Users.AddRange(await _userService.GetAll());
    }
}
