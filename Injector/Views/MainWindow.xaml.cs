﻿using Injector.Views;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Injector
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        public MainWindow()
        {
            try
            {
                AppLanguages.LoadDefaultLanguage();
                AppTheme.LoadDefaltTheme();
                //if (Properties.Settings.Default.SN == null || Properties.Settings.Default.SN.Length < 1)
                //{
                //    LicenseActivation license = new LicenseActivation();
                //    license.ShowDialog();
                //    if (license.DialogResult == false)
                //    {
                //        Application.Current.Shutdown();
                //    }
                //}
                InitializeComponent();
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Source + '\n' +  ex.Message + '\n' + ex.StackTrace + '\n' + ex.InnerException);
            }
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        
        private void SwitcherStateApp_Click(object sender, RoutedEventArgs e)
        {
            
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else WindowState = WindowState.Normal;
        }

        private void MinimizeApp_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ControlPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
