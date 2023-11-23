using System;
using System.Collections.Generic;
using System.Collections;
using System.IO.Ports;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Timer = System.Timers.Timer;
using System.IO;
using System.Management;
using System.Windows.Documents;
using System.Drawing.Drawing2D;
using System.Windows.Media;

namespace Injector.ConnectionToMC
{
    public class Connection
    {

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public class DeviceBroadcastInterface
        {
            public int Size;
            public int DeviceType;
            public int Reserved;
            public Guid ClassGuid;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Name;
        }
        /// <summary>
        /// Registers a window for device insert/remove messages
        /// </summary>
        /// <param name="hwnd">Handle to the window that will receive the messages</param>
        /// <param name="oInterface">DeviceBroadcastInterrface structure</param>
        /// <param name="nFlags">set to DEVICE_NOTIFY_WINDOW_HANDLE</param>
        /// <returns>A handle used when unregistering</returns>
        [DllImport("user32.dll", SetLastError = true)] public static extern IntPtr RegisterDeviceNotification(IntPtr hwnd, DeviceBroadcastInterface oInterface, uint nFlags);
        /// <summary>
        /// Unregister from above.
        /// </summary>
        /// <param name="hHandle">Handle returned in call to RegisterDeviceNotification</param>
        /// <returns>True if success</returns>
        [DllImport("user32.dll", SetLastError = true)] public static extern bool UnregisterDeviceNotification(IntPtr hHandle);
        /// <summary>
        /// Gets the GUID that Windows uses to represent HID class devices
        /// </summary>
        /// <param name="gHid">An out parameter to take the Guid</param>
        [DllImport("hid.dll", SetLastError = true)] public static extern void HidD_GetHidGuid(out Guid gHid);

        /// <summary>Used when registering for device insert/remove messages : specifies the type of device</summary>
        protected const int DEVTYP_DEVICEINTERFACE = 0x05;
        /// <summary>Used when registering for device insert/remove messages : we're giving the API call a window handle</summary>
        protected const int DEVICE_NOTIFY_WINDOW_HANDLE = 0;
        /// <summary>Windows message sent when a device is inserted or removed</summary>
        public const int WM_DEVICECHANGE = 0x0219;
        /// <summary>WParam for above : A device was inserted</summary>
        public const int DEVICE_ARRIVAL = 0x8000;
        /// <summary>WParam for above : A device was removed</summary>
        public const int DEVICE_REMOVECOMPLETE = 0x8004;

        private static readonly byte[] m_checkRequest = { 0xfd };
        private string m_currentPortName;

        private string m_buffer = string.Empty;
        private int m_ping = 0;

        private static Timer m_checkConnection;
        private string[] m_list;
        private static List<string> m_portsList = new List<string>();

        public static List<string> DisplayingExistDevices()
        {
            return m_portsList;
        }

        public static void ChangeExistDevices(char symbol, int UID)
        {
            if (symbol == 'A')
            {
                m_portsList.Add($"Injekt-{UID}");
            }    
            else if (symbol == 'R')
            {
                m_portsList.Remove($"Injekt-{UID}");
            }
        }

        public string GetPortName()
        {
            return m_currentPortName;
        }
        private void SetPortName(string name)
        {
            m_currentPortName = name;
        }
         
        public bool AutoConnect()
        {
            var allPort = DisplayingExistDevices();
            bool finded = false;
            for (int i = 0; i < allPort.Count; i++)
            {
                finded = true;
                SetPortName(m_portsList[0]);
            }
            return finded;
        }


        private void ReadingResponse(object sender, SerialDataReceivedEventArgs e)
        {
            //m_buffer += m_comPort.ReadExisting();
            {
                string[] lines = m_buffer.Split('\n');
                m_list = lines;
                int num = m_list.Length -1;
                m_buffer = m_list[num];
                for (int i = 0; i < num; i++)
                {
                    if (m_ping > 0) m_ping--;
                    EventBus.ReceiveResponse(m_list[i]);
                }
            }

        }

        private void DisconnectPort()
        {
            //m_comPort.DataReceived -= ReadingResponse;
            m_checkConnection.Elapsed -= Callback;
            EventBus.Disconnecting();
            //m_comPort.Close();
            //MessageBox.Show("Disconnected");
        }

        private void CheckerStart()
        {
            m_ping = 0;
            m_checkConnection = new Timer();
            m_checkConnection.Elapsed += Callback;
            m_checkConnection.Interval = 1000;
            m_checkConnection.AutoReset = true;
            m_checkConnection.Enabled = true;

        }
        private void Callback(object sender, ElapsedEventArgs e)
        {
            if (/*m_comPort.IsOpen == false ||*/ m_ping > 2)
            {
                DisconnectPort();
            }
            else
            {
                m_ping++;
                SendRequest("V2\n");
            }
        }
        public void SendRequest(string request)
        {
            try
            {
                //m_comPort.Write(request);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        
    }
}
