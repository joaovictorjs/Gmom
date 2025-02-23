using System.Windows;
using Gmom.Domain.Interface;
using Gmom.Presentation.ViewModels;
using MahApps.Metro.Controls;

namespace Gmom.Presentation.Views;

public partial class LoginView : MetroWindow
{
    public LoginView()
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
