using System.Windows;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;
using Gmom.Infrastructure.Stores;
using Gmom.Presentation.Views;

namespace Gmom.Presentation.ViewModels;

public class MainViewModel : BindableBase, IClosableWindow
{
    private readonly ICurrentUserStore _currentUserStore;
    private readonly IWindowService<LoginView, LoginViewModel> _loginViewService;

    public DelegateCommand LogoutCommand { get; }

    public MainViewModel(
        ICurrentUserStore currentUserStore,
        IWindowService<LoginView, LoginViewModel> loginViewService
    )
    {
        _currentUserStore = currentUserStore;
        _loginViewService = loginViewService;

        LogoutCommand = new DelegateCommand(Logout);
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

    public Action? Close { get; set; }
}
