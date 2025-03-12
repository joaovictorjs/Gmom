using Gmom.Domain.Interface;
using Gmom.Domain.Models;
using Gmom.Presentation.Events;
using Gmom.Presentation.Views;

namespace Gmom.Presentation.ViewModels;

public class MainViewModel : BindableBase, IClosableWindow
{
    private readonly ICurrentUserStore _currentUserStore;
    private readonly IWindowService<LoginView, LoginViewModel> _loginViewService;
    private readonly IWindowService<ManageUsersView, ManageUserViewModel> _manageUsersService;

    private string _currentFlyout = string.Empty;

    public DelegateCommand OpenManageUsersCommand { get; }

    public MainViewModel(
        ICurrentUserStore currentUserStore,
        IWindowService<LoginView, LoginViewModel> loginViewService,
        IEventAggregator eventAggregator,
        IWindowService<ManageUsersView, ManageUserViewModel> manageUsersService
    )
    {
        _currentUserStore = currentUserStore;
        _loginViewService = loginViewService;
        _manageUsersService = manageUsersService;

        LogoutCommand = new DelegateCommand(Logout);
        OpenManageUsersCommand = new DelegateCommand(OpenManageUsers);

        eventAggregator.GetEvent<FlyoutOpened>().Subscribe(FlyoutOpened);
    }

    private void OpenManageUsers()
    {
        _manageUsersService.Create().ShowDialog();
    }

    public string CurrentFlyout
    {
        get => _currentFlyout;
        set => SetProperty(ref _currentFlyout, value);
    }

    public DelegateCommand LogoutCommand { get; }

    public Action? Close { get; set; }

    private void FlyoutOpened(string flyoutName)
    {
        CurrentFlyout = CurrentFlyout == flyoutName ? string.Empty : flyoutName;
    }

    private void Logout()
    {
        _currentUserStore.Value = new UserModel
        {
            Id = 0,
            Name = string.Empty,
            Password = string.Empty,
            IsAdmin = false,
        };

        _loginViewService.Create().Show();
        Close?.Invoke();
    }
}
