using Client.Business.Extensions.ViewModels;
using Client.Business.Extensions.ViewModels.Features.Group;
using Client.Business.Extensions.ViewModels.Features.User;
using Client.Business.ViewModels.Features.Setting;
using Client.Business.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Business.Extensions;

public static class ViewModelExtension
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<GroupMenuViewModel>();
        services.AddSingleton<SettingsMenuViewModel>();
        services.AddSingleton<UserMenuViewModel>();


        services.AddSingleton<UserViewModel>();
        services.AddSingleton<SettingViewModel>();
        services.AddSingleton<GroupViewModel>();
        services.AddSingleton<MemberUserGroupViewModel>();



        return services;
    }
}