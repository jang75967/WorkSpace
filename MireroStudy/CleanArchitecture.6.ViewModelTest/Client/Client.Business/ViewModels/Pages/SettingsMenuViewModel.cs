using Client.Business.ViewModels.Features.Setting;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.Business.ViewModels.Pages;

public partial class SettingsMenuViewModel : ObservableObject
{
    [ObservableProperty]
    public object _currentViewModel;
    public SettingsMenuViewModel(SettingViewModel settingViewModel)
    {
        CurrentViewModel = settingViewModel;
    }
}
