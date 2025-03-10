using System.Windows;
using Autofac;
using Gmom.Domain.Interface;

namespace Gmom.Presentation.Services;

public class WindowService<TWindow, TViewModel>(IComponentContext componentContext)
    : IWindowService<TWindow, TViewModel>
    where TWindow : Window
    where TViewModel : class
{
    private TViewModel? _viewModel;
    private TWindow? _window;

    public IWindowService<TWindow, TViewModel> Create(
        params KeyValuePair<string, object?>[] namedParameters
    )
    {
        _window = componentContext.Resolve<TWindow>();
        var parameters = namedParameters
            .ToList()
            .Select(pair => new NamedParameter(pair.Key, pair.Value));
        _viewModel = componentContext.Resolve<TViewModel>(parameters);
        _window.DataContext = _viewModel;
        
        if(_viewModel is IClosableWindow closableWindow)
            closableWindow.Close = _window.Close;
        
        return this;
    }

    public IWindowService<TWindow, TViewModel> Create()
    {
        _window = componentContext.Resolve<TWindow>();
        return this;
    }

    public bool? ShowDialog()
    {
        return _window?.ShowDialog();
    }

    public void Show()
    {
        _window?.Show();
    }
}