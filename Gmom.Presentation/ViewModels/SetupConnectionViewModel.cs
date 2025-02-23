using Gmom.Domain.Interface;
using Gmom.Domain.Models;
using Gmom.Infrastructure.Exceptions;
using Gmom.Presentation.Views;

namespace Gmom.Presentation.ViewModels;

public class SetupConnectionViewModel : BindableBase, IClosableWindow
{
    private readonly IConnectionFileService _connectionFileService;
    private readonly IPostgresConnectionStore _connectionStore;
    private readonly IMessageBoxService _messageBoxService;
    private readonly IMigrationService _migrationService;
    private readonly IWindowService<LoginView, LoginViewModel> _windowService;
    private string _database;

    private string _host;
    private bool _isLoading;
    private string _password;
    private string _port;
    private string _username;

    public SetupConnectionViewModel(
        IMessageBoxService messageBoxService,
        IMigrationService migrationService,
        IConnectionFileService connectionFileService,
        IPostgresConnectionStore connectionStore,
        IWindowService<LoginView, LoginViewModel> windowService
    )
    {
        _messageBoxService = messageBoxService;
        _migrationService = migrationService;
        _connectionFileService = connectionFileService;
        _connectionStore = connectionStore;
        _windowService = windowService;

        _host = _connectionStore.Value.Host;
        _port = _connectionStore.Value.Port;
        _database = _connectionStore.Value.Database;
        _username = _connectionStore.Value.Username;
        _password = _connectionStore.Value.Password;

        SaveCommand = new AsyncDelegateCommand(Save, CanSave);
    }

    public string Host
    {
        get => _host;
        set => SetProperty(ref _host, value);
    }

    public string Port
    {
        get => _port;
        set => SetProperty(ref _port, value);
    }

    public string Database
    {
        get => _database;
        set => SetProperty(ref _database, value);
    }

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public bool IsFormVisible => !IsLoading;

    public AsyncDelegateCommand SaveCommand { get; }

    public Action? Close { get; set; }

    private bool CanSave()
    {
        return !string.IsNullOrWhiteSpace(Host)
               && !string.IsNullOrWhiteSpace(Port)
               && !string.IsNullOrWhiteSpace(Database)
               && !string.IsNullOrWhiteSpace(Username)
               && !string.IsNullOrWhiteSpace(Password)
               && !IsLoading;
    }

    private async Task Save()
    {
        IsLoading = true;

        try
        {
            _connectionStore.Value = new PostgresConnectionModel
            {
                Host = Host,
                Port = Port,
                Database = Database,
                Username = Username,
                Password = Password
            };

            await _migrationService.MigrateAsync();
            _connectionFileService.Write();

            _windowService.Create().Show();
            Close?.Invoke();
        }
        catch (Exception ex)
        {
            ex.UseGlobalHandle(_messageBoxService.ShowError);
        }
        finally
        {
            IsLoading = false;
        }
    }

    protected override bool SetProperty<T>(ref T storage, T value, string? propertyName = null)
    {
        SaveCommand.RaiseCanExecuteChanged();
        return base.SetProperty(ref storage, value, propertyName);
    }
}