using System.Collections.ObjectModel;
using System.Windows;
using Gmom.Domain.Interface;
using Gmom.Domain.Models;
using Gmom.Infrastructure.Stores;
using Gmom.Presentation.Events;
using Gmom.Presentation.Views;
using MahApps.Metro.Controls;

namespace Gmom.Presentation.ViewModels;

public class MainViewModel : BindableBase, IClosableWindow
{
    private readonly ICurrentUserStore _currentUserStore;
    private readonly IWindowService<LoginView, LoginViewModel> _loginViewService;

    private string _currentFlyout = string.Empty;

    public string CurrentFlyout
    {
        get => _currentFlyout;
        set => SetProperty(ref _currentFlyout, value);
    }

    public DelegateCommand LogoutCommand { get; }

    public MainViewModel(
        ICurrentUserStore currentUserStore,
        IWindowService<LoginView, LoginViewModel> loginViewService,
        IEventAggregator eventAggregator
    )
    {
        _currentUserStore = currentUserStore;
        _loginViewService = loginViewService;

        LogoutCommand = new DelegateCommand(Logout);

        eventAggregator.GetEvent<FlyoutOpened>().Subscribe(FlyoutOpened);
    }

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

    public Action? Close { get; set; }
}
