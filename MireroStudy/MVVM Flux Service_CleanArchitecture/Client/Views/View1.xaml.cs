using Client.Business.Domain.ViewInterface;
using System.Windows;

namespace Client.Views;

public partial class View1 : Window, IView1Dialog
{
    public View1()
    {
        InitializeComponent();
    }
}
