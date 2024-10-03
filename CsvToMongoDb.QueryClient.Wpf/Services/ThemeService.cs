using System.Windows;
using ControlzEx.Theming;
using CsvToMongoDb.QueryClient.Wpf.Models;

namespace CsvToMongoDb.QueryClient.Wpf.Services;

public class ThemeService : IThemeService
{
    private readonly IUserSettingsService _userSettingsService;

    public ThemeService(IUserSettingsService userSettingsService)
    {
        _userSettingsService = userSettingsService;
    }

    public void ChangeTheme(string theme)
    {
        _userSettingsService.UpdateUserSettings(settings => settings.SelectedTheme = theme);
        switch (theme)
        {
            case "Light":
                ThemeManager.Current.ChangeTheme(Application.Current, "Light", "Blue");
                break;
            case "Dark":
                ThemeManager.Current.ChangeTheme(Application.Current, "Dark", "Blue");
                break;
        }
    }
}