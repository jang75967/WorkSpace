using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WPFViewNavigation.Services;
using WPFViewNavigation.Stores;
using WPFViewNavigation.ViewModels;

namespace WPFViewNavigation;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public new static App Current => (App)Application.Current;

    private IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Stores
        services.AddSingleton<MainNavigationStore>();
        services.AddSingleton<SignupStore>();
        services.AddSingleton<LeftStore>();
        services.AddSingleton<RightStore>();

        // Services
        services.AddSingleton<INavigationService, NavigationService>();

        // ViewModels
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<SignupViewModel>();
        services.AddSingleton<LeftViewModel>();
        services.AddSingleton<RightViewModel>();
        services.AddSingleton<TestViewModel>();

        // Views
        services.AddSingleton(s => new MainWindow()
        {
            DataContext = s.GetRequiredService<MainViewModel>()
        });

        return services.BuildServiceProvider();
    }

    public App()
    {
        Services = ConfigureServices();

        var mainView = Services.GetRequiredService<MainWindow>();
        mainView.Show();
    }

    public IServiceProvider Services { get; }
}
