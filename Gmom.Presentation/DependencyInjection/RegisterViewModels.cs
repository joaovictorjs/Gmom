using Autofac;
using Gmom.Presentation.ViewModels;

namespace Gmom.Presentation.DependencyInjection;

public partial class Injector
{
    private static void RegisterViewModels()
    {
        Builder.RegisterType<SetupConnectionViewModel>();

        Builder.RegisterType<LoginViewModel>();

        Builder.RegisterType<MainViewModel>();

        Builder.RegisterType<SessionViewModel>();

        Builder.RegisterType<TilesViewModel>();
        
        Builder.RegisterType<LocateProductsViewModel>();
        
        Builder.RegisterType<InsertOrUpdateProductViewModel>();
        
        Builder.RegisterType<ManageUserViewModel>();
    }
}