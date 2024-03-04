using Client_WPFStream.Models;
using RedisLibrary;
using System.Windows;
using System.Windows.Input;

namespace Client_WPFStream
{
    public class MainViewModel : PropertyChangedHelper
    {

        public string InputStream
        {
            get { return _inputStream; }
            set { SetField(ref _inputStream, value, "ItemsSource"); }
        }

        public string InputField
        {
            get { return _inputField; }
            set { SetField(ref _inputField, value, "ItemsSource"); }
        }

        public string InputValue
        {
            get { return _inputValue; }
            set { SetField(ref _inputValue, value, "ItemsSource"); }
        }

        private ObservableRangeCollection<StreamItem> _streamItems = new();
        public ObservableRangeCollection<StreamItem> StreamItems
        {
            get { return _streamItems; }
            set { SetField(ref _streamItems, value, "ItemsSource"); }
        }

        private StreamItem _selectedItem;
        public StreamItem SelectedItem
        {
            get { return _selectedItem; }
            set { SetField(ref _selectedItem, value, "ItemsSource"); }
        }

        private ObservableRangeCollection<StreamItem> _streamGroupItems = new();
        public ObservableRangeCollection<StreamItem> StreamGroupItems
        {
            get { return _streamGroupItems; }
            set { SetField(ref _streamGroupItems, value, "ItemsSource"); }
        }

        private StreamItem _selectedGroupItem;
        public StreamItem SelectedGroupItem
        {
            get { return _selectedGroupItem; }
            set { SetField(ref _selectedGroupItem, value, "ItemsSource"); }
        }

        public ICommand RemoveCommand { get; set; }
        public ICommand InputCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand StreamInfoCommand {  get; set; }
        public ICommand CreateGroupCommand { get; set; }
        public ICommand ConsumerCommand { get; private set; }
        public ICommand AckGroupCommand { get; private set; }

        private string _inputStream = string.Empty;
        private string _inputField = string.Empty;
        private string _inputValue = string.Empty;
        private IStream _stream = null;

        public MainViewModel()
        {
            RemoveCommand = new DelegateCommand(async () => await RemoveCommandAction());
            InputCommand = new DelegateCommand(async () => await InputCommandAction());
            RefreshCommand = new DelegateCommand(async () => await RefreshCommandAction());
            StreamInfoCommand = new DelegateCommand(async () => await StreamInfoCommandAction());
            CreateGroupCommand = new DelegateCommand(async () => await CreateGroupCommandAction());
            ConsumerCommand = new DelegateCommand(async () => await ConsumerCommandAction());
            AckGroupCommand = new DelegateCommand(async () => await AckGroupCommandAction());

            IAddress address = new Address("127.0.0.1", "6379");
            IConfiguration configuration = new StreamConfiguration(address);
            IConnectionFactory connectionFactory = new ConnectionFactory(configuration);
            _stream = new Stream(connectionFactory);
        }

        private async Task InputCommandAction()
        {
            if (InputStream != null)
            {
                await _stream.AddStreamAsync(InputStream, InputField, InputValue);
                await RefreshCommandAction();
            }
        }

        private async Task RemoveCommandAction()
        {
            if (SelectedItem != null)
            {
                await _stream.RemoveAsync(SelectedItem.Id, SelectedItem.StreamName);
                await RefreshCommandAction();
            }
        }

        private async Task RefreshCommandAction()
        {
            StreamItems.Clear();
            var streams = await _stream.GetAllStreamsAsync(InputStream);
            var itemsToAdd = streams.Select(x => new StreamItem(x.Id, InputStream, x.Values[0].Name, x.Values[0].Value)).ToList();
            StreamItems.AddRange(itemsToAdd);
        }

        private async Task StreamInfoCommandAction()
        {
            string message = await _stream.StreamInfoAsync(InputStream);
            MessageBox.Show($"{message}");
        }

        private async Task CreateGroupCommandAction()
        {
            bool result = await _stream.StreamCreateConsumerGroupAsync(InputStream, "mygroup");
        }

        private async Task ConsumerCommandAction()
        {
            var consumerMessage = await _stream.StreamReadGroupAsync(InputStream, "mygroup", "beomConsumer", 5);
            var itemsToAdd = consumerMessage.Select(x => new StreamItem(x.Id, InputStream, x.Values[0].Name, x.Values[0].Value)).ToList();
            StreamGroupItems.AddRange(itemsToAdd);
        }

        private async Task AckGroupCommandAction()
        {
            var pendingMessages = await _stream.StreamPendingMessagesAsync(InputStream, "mygroup", 5, "beomConsumer", "0-0");
            if (SelectedGroupItem != null)
            {
                foreach (var message in pendingMessages)
                {
                    if (message.MessageId == SelectedGroupItem.Id)
                    {
                        await _stream.StreamAcknowledgeAsync(InputStream, "mygroup", message.MessageId);
                    }
                }
            }
        }
    }

}
