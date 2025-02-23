using Gmom.Domain.Interface;
using Gmom.Infrastructure.Exceptions;
using Gmom.Presentation.Views;

namespace Gmom.Presentation.ViewModels;

public class LoginViewModel : BindableBase, IClosableWindow
{
    private readonly IUserService _userService;
    private readonly IMessageBoxService _messageBoxService;
    private readonly IWindowService<
        SetupConnectionView,
        SetupConnectionViewModel
    > _connectionViewService;
    private readonly IWindowService<MainView, MainViewModel> _mainViewService;
    private readonly ICurrentUserStore _currentUserStore;

    private string _name = string.Empty;
    private string _password = string.Empty;

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

    public DelegateCommand OpenSetupDatabaseViewCommand { get; }
    public AsyncDelegateCommand LoginCommand { get; }

    public LoginViewModel(
        IConnectionFileService connectionFileService,
        IMessageBoxService messageBoxService,
        IWindowService<SetupConnectionView, SetupConnectionViewModel> connectionViewService,
        IUserService userService,
        IWindowService<MainView, MainViewModel> mainViewService,
        ICurrentUserStore currentUserStore
    )
    {
        _messageBoxService = messageBoxService;
        _connectionViewService = connectionViewService;
        _userService = userService;
        _mainViewService = mainViewService;
        _currentUserStore = currentUserStore;

        connectionFileService.Read();

        OpenSetupDatabaseViewCommand = new DelegateCommand(OpenSetupDatabaseView);
        LoginCommand = new AsyncDelegateCommand(Login, CanLogin);
    }

    private bool CanLogin()
    {
        return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Password);
    }

    private async Task Login()
    {
        try
        {
            _currentUserStore.Value = await _userService.Login(Name, Password);
            _mainViewService.Create().Show();
            Close?.Invoke();
        }
        catch (Exception ex)
        {
            ex.UseGlobalHandle(_messageBoxService.ShowError);
        }
    }

    private void OpenSetupDatabaseView()
    {
        _connectionViewService.Create().Show();
        Close?.Invoke();
    }

    protected override bool SetProperty<T>(ref T storage, T value, string? propertyName = null)
    {
        LoginCommand.RaiseCanExecuteChanged();
        return base.SetProperty(ref storage, value, propertyName);
    }

    public Action? Close { get; set; }
}
