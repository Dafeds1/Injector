using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Injector.Views
{
    /// <summary>
    /// Логика взаимодействия для MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public MessageWindow()
        {
            InitializeComponent();
            
        }
        public MessageWindow(string message, string caption, MessageBoxButton buttons)
        {
            InitializeComponent();
            Message.Text = message;
            Caption.Text = caption;
            if (buttons == MessageBoxButton.OK)
            {
                Cancel.Visibility = Visibility.Collapsed;
                Yes.Visibility = Visibility.Collapsed;
                No.Visibility = Visibility.Collapsed;
            }
            if (buttons == MessageBoxButton.YesNo)
            {
                Cancel.Visibility = Visibility.Collapsed;
                Ok.Visibility = Visibility.Collapsed;
            }
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;  
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = false;
        }
    }
}
