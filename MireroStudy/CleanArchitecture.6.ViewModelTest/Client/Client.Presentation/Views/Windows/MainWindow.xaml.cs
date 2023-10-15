using Client.Business.Core.Domain.Events.Retry;
using Client.Business.Core.Domain.Events.Settings;
using Client.Business.Extensions.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls; // 이걸 viewModel에서 참조하는게 과연 맞는걸까?
using Wpf.Ui.Services;

namespace Client.Presentation.Views.Windows
{
    public partial class MainWindow
    {
        public MainViewModel ViewModel { get; }

        public MainWindow(
            MainViewModel viewModel,
            IMessenger messenger,
            INavigationService navigationService,
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService,
            IContentDialogService contentDialogService,
            IThemeService themeService
        )
        {
            Wpf.Ui.Appearance.Watcher.Watch(this);

            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            navigationService.SetNavigationControl(NavigationView);
            snackbarService.SetSnackbarPresenter(SnackbarPresenter);
            contentDialogService.SetContentPresenter(RootContentDialog);

            NavigationView.SetServiceProvider(serviceProvider);

            messenger.Register<ThemeSwitchEvent>(this, (r, e) =>
            {
                var theme = e.theme == Theme.Light ? Wpf.Ui.Appearance.ThemeType.Light : Wpf.Ui.Appearance.ThemeType.Dark;
                themeService.SetTheme(theme);
            });

            messenger.Register<RetryErrorEvent>(this, (r, e) =>
            {

                snackbarService.Show("Data Retrieval Failure", e.Message,
                    Wpf.Ui.Controls.ControlAppearance.Caution,
                    new SymbolIcon(Wpf.Ui.Common.SymbolRegular.Warning12),
                    TimeSpan.FromSeconds(3));
            });
        }
    }
}
