using System.Diagnostics;
using System.Windows;

namespace ConfigurateWatiTest
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

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            //txtConfigAwait.Text = "Before configure Await";

            //await DoSomethingAsync().ConfigureAwait(false);

            //txtConfigAwait.Text = "After configure await set to false";

            DoSomethingAsync().Wait();
        }

        private async Task DoSomethingAsync()
        {
            await Task.Delay(1000).ConfigureAwait(false);   

            Debug.WriteLine("Continuation task");
        }
    }
}