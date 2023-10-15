using Client.Business.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace Client.Presentation.Views.Pages;

/// <summary>
/// GroupPage.xaml에 대한 상호 작용 논리
/// </summary>
public partial class GroupMenuPage : INavigableView<GroupMenuViewModel>
{
    public GroupMenuViewModel ViewModel { get; }
    public GroupMenuPage(GroupMenuViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }
}
