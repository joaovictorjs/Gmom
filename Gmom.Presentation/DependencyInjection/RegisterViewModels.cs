using Autofac;
using Gmom.Presentation.ViewModels;

namespace Gmom.Presentation.DependencyInjection;

public partial class Injector
{
    private static void RegisterViewModels()
    {
        Builder.RegisterType<SetupConnectionViewModel>();

        Builder.RegisterType<InsertOrUpdateUserViewModel>();
    }
}
