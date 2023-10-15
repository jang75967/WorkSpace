using Client.Business.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace Client.Presentation.Views.Pages;

/// <summary>
/// UserPage.xaml에 대한 상호 작용 논리
/// </summary>
public partial class UserMenuPage : INavigableView<UserMenuViewModel>
{
    public UserMenuViewModel ViewModel { get; }
    public UserMenuPage(UserMenuViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }
}
