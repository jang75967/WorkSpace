using Client_WPF.Models;
using RedisLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client_WPF
{
    public class MainViewModel : PropertyChangedHelper
    {
       
        public string InputText
        {
            get { return _inputText; }
            set { SetField(ref _inputText, value, "ItemsSource"); }
        }

        private ObservableRangeCollection<QueueItem> _queueItems = new ObservableRangeCollection<QueueItem>();
        public ObservableRangeCollection<QueueItem> QueueItems
        {
            get { return _queueItems; }
            set { SetField(ref _queueItems, value, "ItemsSource"); }
        }

        private QueueItem _selectedItem;
        public QueueItem SelectedItem
        {
            get { return _selectedItem; }
            set { SetField(ref _selectedItem, value, "ItemsSource"); }
        }

        public ICommand RemoveCommand { get; set; }
        public ICommand InputCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        private string _inputText = string.Empty;
        private IQueue _queue = null;

        public MainViewModel()
        {
            RemoveCommand = new DelegateCommand(RemoveCommandAction);
            InputCommand = new DelegateCommand(InputCommandAction);
            RefreshCommand = new DelegateCommand(RefreshCommandAction);

            IAddress address = new Address("127.0.0.1", "6379");
            IConfiguration configuration = new Configuration(address, "test-queue");
            IConnectionFactory connectionFactory = new ConnectionFactory(configuration);
            _queue = new Queue(connectionFactory);
        }



        private void InputCommandAction()
        {
            if (InputText != null)
            {
                _queue.Enqueue(InputText);
                RefreshCommandAction();
            }
        }

        private void RemoveCommandAction()
        {
            if (SelectedItem != null)
            {
                _queue.Remove(SelectedItem.Value);
                RefreshCommandAction();
            }
                
        }

        private void RefreshCommandAction()
        {
            QueueItems.Clear();
            int itemIndex = 0;
            QueueItems.AddRange(_queue.GetAllItems().Select(x => new QueueItem(itemIndex++, x)).ToList());
        }
    }

}
