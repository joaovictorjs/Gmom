using Autofac;

namespace Gmom.Presentation.DependencyInjection;

public partial class Injector
{
    private static Injector? _injector;
    public static Injector Instance => _injector ??= new Injector();
    private readonly ContainerBuilder _builder = new();
    private readonly IContainer _container;

    private Injector()
    {
        MakeRegistrations();
        _container = _builder.Build();
    }

    private static void MakeRegistrations()
    {
        throw new NotImplementedException();
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
