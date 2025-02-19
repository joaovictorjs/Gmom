using Autofac;

namespace Gmom.Presentation.DependencyInjection;

public partial class Injector
{
    private static Injector? _injector;
    public static Injector Instance => _injector ??= new Injector();
    private static readonly ContainerBuilder Builder = new();
    private readonly IContainer _container;

    private Injector()
    {
        MakeRegistrations();
        _container = Builder.Build();
    }

    private static void MakeRegistrations()
    {
        RegisterServices();
        RegisterViews();
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
