using System.Windows;
using System.Windows.Controls;

namespace CsvToMongoDb.QueryClient.Views;

public static class TextBoxExtensions
{
    public static readonly DependencyProperty AutoScrollToEndProperty = DependencyProperty.RegisterAttached(
        "AutoScrollToEnd",
        typeof(bool),
        typeof(TextBoxExtensions),
        new PropertyMetadata(default(bool), OnAutoScrollToEndChanged));

    private static void OnAutoScrollToEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TextBox textBox && e.NewValue is bool newValue && newValue)
        {
            textBox.TextChanged += (s, e) => textBox.ScrollToEnd();
        }
    }

    public static void SetAutoScrollToEnd(TextBox element, bool value)
    {
        element.SetValue(AutoScrollToEndProperty, value);
    }

    public static bool GetAutoScrollToEnd(TextBox element)
    {
        return (bool)element.GetValue(AutoScrollToEndProperty);
    }
}