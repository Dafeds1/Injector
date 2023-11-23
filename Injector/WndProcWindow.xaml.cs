using HIDInterface;
using Injector.ConnectionToMC;
using Injector.Views;
using System;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Windows;
using System.Windows.Interop;

namespace Injector
{
    /// <summary>
    /// Логика взаимодействия для WndProc.xaml
    /// </summary>
    public partial class WndProcWindow : Window
    {
        private HIDDevice Probe;
        /// <summary>Used when registering for device insert/remove messages : specifies the type of device</summary>
        protected const int DEVTYP_DEVICEINTERFACE = 0x05;
        /// <summary>Used when registering for device insert/remove messages : we're giving the API call a window handle</summary>
        protected const int DEVICE_NOTIFY_WINDOW_HANDLE = 0;
        int UnicID;
        int Serial;
        public WndProcWindow()
        {
            InitializeComponent();
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            //HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            source.AddHook(WndProc);

            Guid gHid;
            Connection.HidD_GetHidGuid(out gHid);

            Connection.DeviceBroadcastInterface oInterfaceIn = new Connection.DeviceBroadcastInterface();
            oInterfaceIn.Size = Marshal.SizeOf(oInterfaceIn);
            oInterfaceIn.ClassGuid = gHid;
            oInterfaceIn.DeviceType = DEVTYP_DEVICEINTERFACE;
            oInterfaceIn.Reserved = 0;
            Connection.RegisterDeviceNotification(source.Handle, oInterfaceIn, DEVICE_NOTIFY_WINDOW_HANDLE);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg !=Connection.WM_DEVICECHANGE) return IntPtr.Zero;

            if (wParam.ToInt32() != Connection.DEVICE_ARRIVAL && wParam.ToInt32() != Connection.DEVICE_REMOVECOMPLETE) return IntPtr.Zero;
            int size = Marshal.ReadInt32(lParam, 0);
            int type = Marshal.ReadInt32(lParam, 4);
            if (type != 5) return IntPtr.Zero;   // не HID

            ConnectionTimer_Tick();


            return IntPtr.Zero;
        }
        private void ConnectionTimer_Tick()
        {
            HIDDevice.interfaceDetails[] devices = HIDDevice.getConnectedDevices();

            string devicePath = "";
            for (int i = 0; i < devices.Length; i++)
            {
                if (Probe != null && Probe.productInfo.devicePath == devices[i].devicePath)  // устройство есть и не отключалось
                    return;
                if (devices[i].VID == 0x1209 || devices[i].PID == 0xbabe)
                {
                    // устройство найдено
                    devicePath = devices[i].devicePath;
                    MessageWindow message = new MessageWindow(devices[i].serialNumber, "Ошибка", MessageBoxButton.OK);
                    message.ShowDialog();
                }   
            }

            if (Probe != null)  // убиваем старое подключение
            {
                MessageWindow message = new MessageWindow("Устройство не отвечает, переподключите.", "Ошибка", MessageBoxButton.OK);
                message.ShowDialog();
                Connection.ChangeExistDevices('R', Serial);
                Probe.close();
                Probe = null;
            }

            if (devicePath != "")
            {
                try // попытка подключения
                {
                    Probe = new HIDDevice(devicePath, false);
                    //SendCMD(0, 0);
                    //byte[] answer = Probe.read();
                    //UnicID = (int)BitConverter.ToUInt32(answer, 5);
                    //Serial = (int)BitConverter.ToUInt32(answer, 1);
                    //MessageWindow message = new MessageWindow("Устройство не отвечает, переподключите.", "Ошибка", MessageBoxButton.OK);
                    //message.ShowDialog();
                    Connection.ChangeExistDevices('A', Serial);
                }
                catch
                {
                    Probe.close();
                    Probe = null;
                }
            }
            else // ничего не нашлось
            {
                //Flash.IsEnabled = false;
                //NewSerial.IsEnabled = false;
            }
        }
        private byte[] firmwareData;
        private int firmwareLength;
        private const int PAGE_SIZE = 1024; // размер страницы 1кБ
        private const UInt64 HID_CMD = 0x02444d43444c5442; // BTLDCMD
        private void SendCMD(byte cmd, uint param)
        {
            byte[] request = new byte[13];
            Array.Copy(BitConverter.GetBytes(HID_CMD), 0, request, 0 + 1, 7);
            request[7 + 1] = cmd;
            Array.Copy(BitConverter.GetBytes(param), 0, request, 8 + 1, 4);
            Probe.write(request);
        }
    }
}
