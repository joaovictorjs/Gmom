using System.Windows;
using Autofac;
using Gmom.Domain.Interface;

namespace Gmom.Presentation.Services;

public class WindowService<TWindow, TViewModel>(IComponentContext componentContext)
    : IWindowService<TWindow, TViewModel>
    where TWindow : Window
    where TViewModel : class
{
    private TWindow? _window;
    private TViewModel? _viewModel;

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
        return this;
    }

    public bool? ShowDialog()
    {
        return _window?.ShowDialog();
    }
}
