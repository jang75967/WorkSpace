using Client.Presentation.Views.Pages;
using Client.Presentation.Views.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Presentation.Extentions;

public static class ViewExtension
{
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
        services.AddSingleton<SettingsMenuPage>();
        services.AddSingleton<UserMenuPage>();
        services.AddSingleton<GroupMenuPage>();
        return services;
    }
}
