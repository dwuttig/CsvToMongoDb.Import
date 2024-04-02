using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using MahApps.Metro.Controls;

namespace CsvToMongoDb.QueryClient.Views;

public partial class ShellView : MetroWindow
{
    public ShellView()
    {
        InitializeComponent();
    }

    private void ChangeTheme_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem)
        {
            var theme = menuItem.Header.ToString() ?? "Light";
            Ioc.Default.GetRequiredService<IThemeService>().ChangeTheme(theme);
        }
    }
}