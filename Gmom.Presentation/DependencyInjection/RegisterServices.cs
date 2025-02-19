using Autofac;
using Gmom.Domain.Interface;
using Gmom.Presentation.Services;

namespace Gmom.Presentation.DependencyInjection;

public partial class Injector
{
    private static void RegisterServices()
    {
        Builder.RegisterType<MessageBoxService>().As<IMessageBoxService>().SingleInstance();
    }
}
