using Client.Business.Extensions.ViewModels.Features.User;
using Client.Business.ViewModels.Features.Setting;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Client.Presentation.Views.Features.Settings;

public partial class SettingView : UserControl
{
    public SettingViewModel ViewModel { get; }
    public SettingView()
    {
        ViewModel = App.GetService<SettingViewModel>()!;
        DataContext = this;
  
        InitializeComponent();
    }
}
