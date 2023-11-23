using ScottPlot.Palettes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для LicenseActivation.xaml
    /// </summary>
    public partial class LicenseActivation : Window
    {
        public LicenseActivation()
        {
            InitializeComponent();
        }

        private void CheckSN_Click(object sender, RoutedEventArgs e)
        {
            if (SerialNumber.Text.Length < 1)
            {
                SerialNumber.ToolTip = "Введите серийный номер лицензии";
            }
            if (SerialNumber.Text.Length < 1)
            {
                SerialNumber.ToolTip = "Введите серийный номер лицензии";
            }
        }
    }
}
