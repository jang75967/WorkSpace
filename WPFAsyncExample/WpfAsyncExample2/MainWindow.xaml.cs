using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfAsyncExample2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void RunAynthingAsync(Label lbl)
        {
            for (int i = 0; i < 30; i++)
            {
                await Task.Delay(100);
                lbl.Content = i.ToString();
                lbl.UpdateLayout();
            }
        }

        private void btnWalk_Click(object sender, RoutedEventArgs e)
        {
            RunAynthingAsync(lblWalk);
        }

        private void btnPhone_Click(object sender, RoutedEventArgs e)
        {
            RunAynthingAsync(lblPhone);
        }

        private void btnSpeak_Click(object sender, RoutedEventArgs e)
        {
            RunAynthingAsync(lblSpeak);
        }

        private async Task RunAllAsync(Label lbl)
        {
            for (int i = 0; i < 30; i++)
            {
                await Task.Delay(100);
                lb.Items.Add($"{lbl.Name} - {i}");
            }
        }

        private async void btnList_Click(object sender, RoutedEventArgs e)
        {
            await RunAllAsync(lblWalk);
            await RunAllAsync(lblPhone);
            await RunAllAsync(lblSpeak);
        }
    }
}
