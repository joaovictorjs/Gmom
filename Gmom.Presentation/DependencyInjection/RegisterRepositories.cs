using Autofac;
using Gmom.Domain.Entities;
using Gmom.Domain.Interface;
using Gmom.Infrastructure.Repositories;

namespace Gmom.Presentation.DependencyInjection;

public partial class Injector
{
    private static void RegisterRepositories()
    {
        Builder.RegisterType<Repository>().As<IRepository>();

        Builder.RegisterType<Repository<UserEntity>>().As<IRepository<UserEntity>>();

        Builder.RegisterType<Repository<ProductEntity>>().As<IRepository<ProductEntity>>();
    }
}
