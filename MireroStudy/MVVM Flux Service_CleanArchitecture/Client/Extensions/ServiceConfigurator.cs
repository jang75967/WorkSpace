using Client.Business;
using Client.Business.Domain.ServiceInterface;
using Client.Business.Domain.ViewInterface;
using Client.Service;
using Client.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Extensions;

public static class ServiceConfigurator
{
    public static IServiceCollection Configure(this IServiceCollection services)
    {
        services.Service();
        services.AddViews();
        services.AddViewModels();

        return services;
    }

    private static IServiceCollection Service(this IServiceCollection services)
    {
        services.AddScoped<ICountService, CountService>();
        return services;
    }

    private static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddScoped<MainWindow>();
        services.AddScoped<IView1Dialog, View1>();
        services.AddTransient<IView2Dialog, View2>();
        return services;
    }

    private static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddScoped<MainViewModel>();
        services.AddScoped<View1ViewModel>();
        services.AddScoped<View2ViewModel>();
        return services;
    }

}