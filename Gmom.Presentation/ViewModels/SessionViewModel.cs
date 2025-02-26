using Gmom.Domain.Interface;
using Gmom.Presentation.Views;

namespace Gmom.Presentation.ViewModels;

public class SessionViewModel : BindableBase
{
    private readonly ICurrentUserStore _currentUserStore;

    public string Host { get; }
    public string Port { get; }
    public string Database { get; }
    public string Name { get; }
    public bool IsAdmin { get; }

    public SessionViewModel( ICurrentUserStore currentUserStore,
        IPostgresConnectionStore connectionStore)
    {
        _currentUserStore = currentUserStore;

        Host = connectionStore.Value.Host;
        Port = connectionStore.Value.Port;
        Database = connectionStore.Value.Database;
        Name = currentUserStore.Value.Name;
        IsAdmin = currentUserStore.Value.IsAdmin;
    }
}