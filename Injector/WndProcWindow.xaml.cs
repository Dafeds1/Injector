using Injector.ConnectionToMC;
using System;
using System.Windows;
using System.Windows.Interop;

namespace Injector
{
    /// <summary>
    /// Логика взаимодействия для WndProc.xaml
    /// </summary>
    public partial class WndProcWindow : Window
    {
        public WndProcWindow()
        {
            InitializeComponent();
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            if (source != null)
            {
                IntPtr windowHandle = source.Handle;
                source.AddHook(WndProc);
                Connection.RegisterUsbDeviceNotification(windowHandle);
            }
            
        }
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == Connection.WmDevicechange)
            {
                switch ((int)wparam)
                {
                    case Connection.DbtDeviceremovecomplete:
                        Connection.DisplayingExistPort();// this is where you do your magic
                        break;
                    case Connection.DbtDevicearrival:
                        Connection.DisplayingExistPort();
                        // this is where you do your magic
                        break;
                }
            }
            return IntPtr.Zero;
        }
    }
}
