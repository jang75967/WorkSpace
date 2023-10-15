using Client.Business.Extensions.ViewModels.Features.User;
using Client.Business.ViewModels.Features.Setting;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Client.Presentation.Views.Features.Users;

/// <summary>
/// UserView.xaml에 대한 상호 작용 논리
/// </summary>
public partial class UserView : INavigableView<UserViewModel>
{
    public UserViewModel ViewModel { get; set; } // 테스트 코드용 set 추가
    public UserView()
    {
        ViewModel = App.GetService<UserViewModel>()!;
        DataContext = this;
        InitializeComponent();
    }
}
