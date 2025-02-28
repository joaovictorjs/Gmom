using Gmom.Domain.Interface;

namespace Gmom.Presentation.ViewModels;

public class SessionViewModel : BindableBase
{
    private readonly ICurrentUserStore _currentUserStore;

    public SessionViewModel(ICurrentUserStore currentUserStore,
        IPostgresConnectionStore connectionStore)
    {
        _currentUserStore = currentUserStore;

        Host = connectionStore.Value.Host;
        Port = connectionStore.Value.Port;
        Database = connectionStore.Value.Database;
        Name = currentUserStore.Value.Name;
        IsAdmin = currentUserStore.Value.IsAdmin;
    }

    public string Host { get; }
    public string Port { get; }
    public string Database { get; }
    public string Name { get; }
    public bool IsAdmin { get; }
}