using Autofac;

namespace Gmom.Presentation.DependencyInjection;

public partial class Injector
{
    private static Injector? _injector;
    private static readonly ContainerBuilder Builder = new();
    private readonly IContainer _container;

    private Injector()
    {
        MakeRegistrations();
        _container = Builder.Build();
    }

    public static Injector Instance => _injector ??= new Injector();

    private static void MakeRegistrations()
    {
        RegisterServices();
        RegisterViews();
        RegisterViewModels();
        RegisterStores();
        RegisterFactories();
        RegisterRepositories();

        Builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
    }

    public object Resolve(Type type)
    {
        return _container.Resolve(type);
    }

    public T Resolve<T>()
        where T : notnull
    {
        return _container.Resolve<T>();
    }
}
