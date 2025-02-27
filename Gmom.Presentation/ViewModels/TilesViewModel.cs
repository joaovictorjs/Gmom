using System.Windows;
using Gmom.Presentation.Events;
using MahApps.Metro.Controls;

namespace Gmom.Presentation.ViewModels;

public class TilesViewModel : BindableBase
{
    private IEventAggregator _eventAggregator;
    
    public DelegateCommand<string> OpenFlyoutCommand { get; }
    
    public TilesViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
        
        OpenFlyoutCommand = new DelegateCommand<string>(OpenFlyout);
    }

    private void OpenFlyout(string flyoutName)
    {
        _eventAggregator.GetEvent<FlyoutOpened>().Publish(flyoutName);
    }
}