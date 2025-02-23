using System.Windows;
using Gmom.Domain.Interface;
using MahApps.Metro.Controls;

namespace Gmom.Presentation.Views;

public partial class SetupConnectionView : MetroWindow
{
    public SetupConnectionView()
    {
        InitializeComponent();
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        if (DataContext is IClosableWindow closableWindow)
        {
            closableWindow.Close = Close;
        }
    }
}