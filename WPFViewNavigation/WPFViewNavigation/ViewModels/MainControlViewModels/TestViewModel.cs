using System.Windows.Input;
using WPFViewNavigation.Commands;
using WPFViewNavigation.Services;

namespace WPFViewNavigation.ViewModels;

public class TestViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;

    private void ToLogin(object _)
    {
        _navigationService.Navigate(NaviType.LoginView);
    }

    private void ToSignup(object _)
    {
        _navigationService.Navigate(NaviType.SignupView);
    }

    public TestViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        LeftViewModel = (ViewModelBase)App.Current.Services.GetService(typeof(LeftViewModel))!;
        RightViewModel = (ViewModelBase)App.Current.Services.GetService(typeof(RightViewModel))!;
        ToLoginCommand = new RelayCommand<object>(ToLogin);
        ToSignupCommand = new RelayCommand<object>(ToSignup);
    }

    public ViewModelBase LeftViewModel { get; set; }
    public ViewModelBase RightViewModel { get; set; }
    public ICommand ToLoginCommand { get; set; }
    public ICommand ToSignupCommand { get; set; }
}
