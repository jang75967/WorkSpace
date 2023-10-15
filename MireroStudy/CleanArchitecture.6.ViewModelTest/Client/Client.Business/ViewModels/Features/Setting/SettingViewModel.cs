using Client.Business.Core.Domain.Events.Settings;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Client.Business.ViewModels.Features.Setting;

public partial class SettingViewModel : ObservableObject
{
    private readonly IMessenger _messenger;
    [ObservableProperty]
    private Theme _currentThemeSwitch = Theme.Dark;

    public SettingViewModel(IMessenger messenger)
    {
        _messenger = messenger;
    }

    [RelayCommand]
    private void ThemeSwitch()
    {
        CurrentThemeSwitch = CurrentThemeSwitch == Theme.Light ? Theme.Dark : Theme.Light;
        var @event = new ThemeSwitchEvent(CurrentThemeSwitch);
        _messenger.Send(@event);
    }
}
