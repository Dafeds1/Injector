using System.Windows.Controls;


namespace Injector.Views
{
    /// <summary>
    /// Логика взаимодействия для AdditionalyView.xaml
    /// </summary>
    public partial class AdditionallyView : UserControl
    {
        public AdditionallyView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Calibration calibration = new Calibration();
            calibration.Show();
        }
    }
}
