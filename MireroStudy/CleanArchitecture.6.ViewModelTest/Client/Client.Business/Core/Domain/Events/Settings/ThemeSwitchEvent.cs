namespace Client.Business.Core.Domain.Events.Settings;

public enum Theme
{
    Light,
    Dark
}
public record ThemeSwitchEvent(Theme theme);
