using System.Windows;
using System.Windows.Input;

namespace Gmom.Presentation.Helpers;

public class ControlHelper
{
    private static readonly DependencyProperty MoveFocusToProperty =
        DependencyProperty.RegisterAttached(
            "MoveFocusTo",
            typeof(UIElement),
            typeof(ControlHelper),
            new UIPropertyMetadata(OnMoveFocusToChanged)
        );

    public static UIElement GetMoveFocusTo(DependencyObject obj)
    {
        return (UIElement)obj.GetValue(MoveFocusToProperty);
    }

    public static void SetMoveFocusTo(DependencyObject obj, UIElement value)
    {
        obj.SetValue(MoveFocusToProperty, value);
    }

    private static void OnMoveFocusToChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e
    )
    {
        if(d is not UIElement control)
            return;
        
        if (e.NewValue is not UIElement element)
        {
            control.KeyDown -= OnMoveFocusKeyDown;
        }
        else
        {
            control.KeyDown += OnMoveFocusKeyDown;
        }
    }

    private static void OnMoveFocusKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter) 
            return;
        
        GetMoveFocusTo((DependencyObject)sender).Focus();
    }
}
