using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui.Contracts;
using Wpf.Ui.Services;

namespace Client.Presentation.Extentions;

public static class WpfUIExtension
{
    public static IServiceCollection AddWpfUI(this IServiceCollection services)
    {
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<ISnackbarService, SnackbarService>();
        services.AddSingleton<IContentDialogService, ContentDialogService>();
        services.AddSingleton<IThemeService, ThemeService>();
        return services;
    }
}
