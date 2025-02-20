using Autofac;
using Gmom.Domain.Interface;
using Gmom.Infrastructure.Stores;

namespace Gmom.Presentation.DependencyInjection;

public partial class Injector
{
    private static void RegisterStores()
    {
        Builder
            .RegisterType<PostgresConnectionStore>()
            .As<IPostgresConnectionStore>()
            .SingleInstance();
    }
}
