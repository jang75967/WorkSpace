using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFAsyncExample
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

        private void RunAnything(Label lbl)
        {
            for (int i = 0; i < 30; i++)
            {
                Thread.Sleep(100);
                lbl.Content = i.ToString();
                lbl.UpdateLayout();
            }
        }

        private async void RunAnythingAsync(Label lbl)
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
            RunAnythingAsync(lblWalk);
        }

        private void btnPhone_Click(object sender, RoutedEventArgs e)
        {
            RunAnythingAsync(lblPhone);
        }

        private void btnSpeak_Click(object sender, RoutedEventArgs e)
        {
            RunAnythingAsync(lblSpeak);
        }
    }
}
