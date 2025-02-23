using System.Collections.Immutable;
using Autofac;
using Gmom.Domain.Interface;
using Gmom.Infrastructure.Services;
using Gmom.Presentation.Services;
using Gmom.Presentation.ViewModels;
using Gmom.Presentation.Views;

namespace Gmom.Presentation.DependencyInjection;

public partial class Injector
{
    private static void RegisterServices()
    {
        Builder.RegisterType<MessageBoxService>().As<IMessageBoxService>().SingleInstance();

        Builder.RegisterType<LoggerService>().As<ILoggerService>().SingleInstance();

        Builder.RegisterType<MigrationService>().As<IMigrationService>();

        Builder.RegisterType<UserService>().As<IUserService>();
    }
}
