using Gmom.Presentation.Events;

namespace Gmom.Presentation.ViewModels;

public class TilesViewModel : BindableBase
{
    private readonly IEventAggregator _eventAggregator;

    public TilesViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;

        OpenFlyoutCommand = new DelegateCommand<string>(OpenFlyout);
    }

    public DelegateCommand<string> OpenFlyoutCommand { get; }

    private void OpenFlyout(string flyoutName)
    {
        _eventAggregator.GetEvent<FlyoutOpened>().Publish(flyoutName);
    }
}