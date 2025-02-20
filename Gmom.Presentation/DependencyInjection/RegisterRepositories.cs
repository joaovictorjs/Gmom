using Autofac;
using Gmom.Domain.Interface;
using Gmom.Infrastructure.Repositories;

namespace Gmom.Presentation.DependencyInjection;

public partial class Injector
{
    private static void RegisterRepositories()
    {
        Builder.RegisterType<Repository>().As<IRepository>();
    }
}