using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Gmom.Presentation.Extensions;

public static class FocusManager
{
    private static readonly DependencyProperty PressEnterToMoveFocusProperty =
        DependencyProperty.RegisterAttached(
            "PressEnterToMoveFocus",
            typeof(bool),
            typeof(FocusManager),
            new UIPropertyMetadata(OnPressEnterMoveFocusChanged)
        );

    public static bool GetPressEnterToMoveFocus(DependencyObject target)
    {
        return (bool)target.GetValue(PressEnterToMoveFocusProperty);
    }

    public static void SetPressEnterToMoveFocus(DependencyObject target, bool value)
    {
        target.SetValue(PressEnterToMoveFocusProperty, value);
    }

    private static void OnPressEnterMoveFocusChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (d is not UIElement element)
            return;

        if ((bool)e.NewValue)
        {
            element.KeyDown += OnKeyDown;
        }
        else
        {
            element.KeyDown -= OnKeyDown;
        }
    }

    private static void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
            return;
        var element = sender as UIElement;
        element?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
    }

    private static readonly DependencyProperty FocusProperty =
        DependencyProperty.RegisterAttached(
            "Focus",
            typeof(bool),
            typeof(FocusManager),
            new UIPropertyMetadata(OnInitialFocusChanged)
        );

    public static bool GetFocus(DependencyObject target)
    {
        return (bool)target.GetValue(FocusProperty);
    }

    public static void SetFocus(DependencyObject target, bool value)
    {
        target.SetValue(FocusProperty, value);
    }

    private static void OnInitialFocusChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if (d is not FrameworkElement element)
            return;

        if (!(bool)e.NewValue)
            return;

        element.Loaded += OnLoaded;
    }

    private static void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement element) return;
        
        element.Focus();
        element.Loaded -= OnLoaded;
    }
}
