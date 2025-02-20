using Autofac;
using Gmom.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Gmom.Presentation.DependencyInjection;

public partial class Injector
{
    private static void RegisterFactories()
    {
        Builder
            .RegisterType<PostgresContextFactory>()
            .As<IDbContextFactory<PostgresContext>>()
            .SingleInstance();
    }
}
