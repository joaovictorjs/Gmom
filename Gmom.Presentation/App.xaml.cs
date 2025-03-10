using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xaml;
using Gmom.Domain.Constants;
using Gmom.Domain.Interface;
using Gmom.Presentation.DependencyInjection;
using Gmom.Presentation.Views;

namespace Gmom.Presentation;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly Injector _injector = Injector.Instance;

    public App()
    {
        DispatcherUnhandledException += HandleUnhandledExceptions;
        ViewModelLocationProvider.SetDefaultViewModelFactory(_injector.Resolve);
        
        FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
            new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        Window initialWindow = File.Exists(Filenames.Connection)
            ? _injector.Resolve<LoginView>()
            : _injector.Resolve<SetupConnectionView>();

        initialWindow.Show();
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