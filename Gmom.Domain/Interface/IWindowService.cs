namespace Gmom.Domain.Interface;

public interface IWindowService<TWindow, TViewModel>
    where TWindow : class
    where TViewModel : class
{
    IWindowService<TWindow, TViewModel> Create(params KeyValuePair<string, object?>[] namedParameters);

    bool? ShowDialog();
}
