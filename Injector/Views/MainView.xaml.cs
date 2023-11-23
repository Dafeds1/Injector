using Injector.ViewModels;
using MySql.Data.MySqlClient;
using MySql.Data;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Injector.Views
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            Series.Plot.Style(ScottPlot.Style.Gray1);
            Diagramm.Plot.Style(ScottPlot.Style.Gray1);
        }

        private void Series_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var series = new WpfPlotViewer(Series.Plot);
            series.Show();
            series.WindowState = WindowState.Maximized;
        }

        private void Diagramm_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var diagramm = new WpfPlotViewer(Diagramm.Plot);
            diagramm.Show();
            diagramm.WindowState = WindowState.Maximized;
        }

        private void Series_Loaded(object sender, RoutedEventArgs e)
        {
            double[] dataX = { 2, 32, 43, 4, 6 };
            double[] dataY = { 6, 33, 23, 16, 34 };
            Series.Plot.AddScatter(dataX, dataY, label: "1");
            var legend = Series.Plot.Legend(enable: true);
            legend.Orientation = ScottPlot.Orientation.Horizontal;
            Series.Refresh();
            //Series.Reset();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageWindow message = new MessageWindow("Ты пидор?", "Отвечай честно", MessageBoxButton.YesNo);
            message.ShowDialog();
        }

    }
}
