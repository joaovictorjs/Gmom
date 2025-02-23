using Autofac;
using Gmom.Presentation.Views;

namespace Gmom.Presentation.DependencyInjection;

public partial class Injector
{
    private static void RegisterViews()
    {
        Builder.RegisterType<SetupConnectionView>();

        Builder.RegisterType<LoginView>();

        Builder.RegisterType<MainView>();
    }
}