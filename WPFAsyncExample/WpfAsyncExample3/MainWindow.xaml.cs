using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System;

namespace WpfAsyncExample3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource tokenSource;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task DoWorkAsync(CancellationToken token)
        {
            //while (true)
            //{
            //    await Task.Delay(100);
            //    int.TryParse(lb.Content.ToString(), out int value);
            //    lb.Content = (++value).ToString();
            //}

            while (!token.IsCancellationRequested)
            {
                await Task.Delay(100);
                int.TryParse(lb.Content.ToString(), out int value);
                lb.Content = (++value).ToString();
            }
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var token = tokenSource.Token;
            await DoWorkAsync(token);
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            tokenSource.Cancel();
        }
    }
}
