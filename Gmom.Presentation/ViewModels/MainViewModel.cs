using System.Windows;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;
using Gmom.Infrastructure.Stores;
using Gmom.Presentation.Views;

namespace Gmom.Presentation.ViewModels;

public class MainViewModel : BindableBase, IClosableWindow
{
    private readonly ICurrentUserStore _currentUserStore;
    private IWindowService<LoginView, LoginViewModel> _loginViewService;

    public string Host { get; }
    public string Port { get; }
    public string Database { get; }
    public string Name { get; }
    public bool IsAdmin { get; }

    public DelegateCommand LogoutCommand { get; }

    public MainViewModel(
        ICurrentUserStore currentUserStore,
        IPostgresConnectionStore connectionStore,
        IWindowService<LoginView, LoginViewModel> loginViewService
    )
    {
        _currentUserStore = currentUserStore;
        _loginViewService = loginViewService;

        Host = connectionStore.Value.Host;
        Port = connectionStore.Value.Port;
        Database = connectionStore.Value.Database;
        Name = currentUserStore.Value.Name;
        IsAdmin = currentUserStore.Value.IsAdmin;

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
