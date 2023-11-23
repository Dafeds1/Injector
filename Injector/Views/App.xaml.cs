using Forsunkov;
using Injector.Properties;
using Injector.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Injector
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

            string path = Environment.GetEnvironmentVariable("Temp") + "\\InjectorTemp\\";
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            if (Settings.Default.pathToLogs.Length < 1)
            {
                Settings.Default.pathToLogs = path;
                Settings.Default.Save();
            }
            if (Settings.Default.LogsDirectory.Length < 1)
            {
                Settings.Default.LogsDirectory = path;
                Settings.Default.Save();
            }
            WndProcWindow wndProc = new WndProcWindow();
            wndProc.Show();
            wndProc.Hide();
            wndProc.ShowActivated = true;
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            GlobalExceptionHandler.SavingSoftwareErrors(e.Exception.Source, e.Exception.Message, e.Exception.StackTrace);
        }

    }
}

