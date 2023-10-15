using Client.Business.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace Client.Presentation.Views.Pages;

/// <summary>
/// SettingPage.xaml에 대한 상호 작용 논리
/// </summary>
public partial class SettingsMenuPage :  INavigableView<SettingsMenuViewModel>
{
    public SettingsMenuViewModel ViewModel { get; }
    public SettingsMenuPage(SettingsMenuViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }
}
