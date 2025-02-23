using Gmom.Domain.Interface;
using Gmom.Presentation.Views;

namespace Gmom.Presentation.ViewModels;

public class LoginViewModel : BindableBase, IClosableWindow
{
    private readonly IMessageBoxService _messageBoxService;
    private readonly IWindowService<SetupConnectionView, SetupConnectionViewModel> _windowService;

    private string _name = string.Empty;
    private string _password = string.Empty;

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

    public DelegateCommand OpenSetupDatabaseViewCommand { get; }
    public AsyncDelegateCommand LoginCommand { get; }

    public LoginViewModel(
        IConnectionFileService connectionFileService,
        IMessageBoxService messageBoxService,
        IWindowService<SetupConnectionView, SetupConnectionViewModel> windowService
    )
    {
        _messageBoxService = messageBoxService;
        _windowService = windowService;

        connectionFileService.Read();

        OpenSetupDatabaseViewCommand = new DelegateCommand(OpenSetupDatabaseView);
        LoginCommand = new AsyncDelegateCommand(Login, CanLogin);
    }

    private bool CanLogin()
    {
        return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Password);
    }

    private Task Login()
    {
        throw new NotImplementedException();
    }

    private void OpenSetupDatabaseView()
    {
        _windowService.Create().Show();
        Close?.Invoke();
    }

    protected override bool SetProperty<T>(ref T storage, T value, string? propertyName = null)
    {
        LoginCommand.RaiseCanExecuteChanged();
        return base.SetProperty(ref storage, value, propertyName);
    }

    public Action? Close { get; set; }
}
