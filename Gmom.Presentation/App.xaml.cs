using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;
using Gmom.Domain.Constants;
using Gmom.Domain.Interface;
using Gmom.Presentation.DependencyInjection;

namespace Gmom.Presentation;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly Injector _injector = Injector.Instance;

    public App()
    {
        DispatcherUnhandledException += HandleUnhandledExceptions;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
    }

    private void HandleUnhandledExceptions(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        var message = e.Exception.Message;
        _injector
            .Resolve<IMessageBoxService>()
            .ShowError(
                $"Ocorreu um erro inesperado! Verifique os arquivo {Filenames.Trace}.\n\n{message}"
            );
        message += e.Exception.InnerException?.Message.Insert(0, "\n\n");
        _injector.Resolve<ILoggerService>().Log(message);
    }
}
