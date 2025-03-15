using System.Diagnostics;
using System.Windows;
using Gmom.Domain.Interface;
using Gmom.Presentation.ViewModels;
using MahApps.Metro.Controls;

namespace Gmom.Presentation.Views;

public partial class InsertOrUpdateProductView : MetroWindow
{
    public InsertOrUpdateProductView()
    {
        InitializeComponent();

        DataContextChanged += OnDataContextChanged;
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        AssignMethods();
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        AssignMethods();
    }

    private void AssignMethods()
    {
        if (DataContext is IClosableWindow closableWindow)
        {
            closableWindow.Close = Close;
        }
    }
}
