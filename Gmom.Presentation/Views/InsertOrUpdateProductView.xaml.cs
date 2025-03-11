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

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (DataContext is IClosableWindow closableWindow)
        {
            closableWindow.Close = Close;
        }
    }
}