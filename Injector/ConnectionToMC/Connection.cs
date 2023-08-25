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
        public const int DbtDevicearrival = 0x8000; // system detected a new device        
        public const int DbtDeviceremovecomplete = 0x8004; // device is gone      
        public const int WmDevicechange = 0x0219; // device change event      
        private const int DbtDevtypDeviceinterface = 5;
        private static readonly Guid GuidDevinterfaceUSBDevice = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED"); // USB devices
        private static IntPtr notificationHandle;

        private SerialPort m_comPort = new SerialPort();

        private static readonly byte[] m_checkRequest = { 0xfd };
        private static Parity m_Parity = Parity.None;
        private static int m_BaudRate = 115200;
        private static int m_DataBits = 8;
        private static StopBits m_StopBits = StopBits.One;
        private string m_currentPortName;

        private string m_buffer = string.Empty;
        private int m_ping = 0;

        private static Timer m_checkConnection;
        private string[] m_list;
        
        /// <summary>
        /// Registers a window to receive notifications when USB devices are plugged or unplugged.
        /// </summary>
        /// <param name="windowHandle">Handle to the window receiving notifications.</param>
        public static void RegisterUsbDeviceNotification(IntPtr windowHandle)
        {
            DevBroadcastDeviceinterface dbi = new DevBroadcastDeviceinterface
            {
                DeviceType = DbtDevtypDeviceinterface,
                Reserved = 0,
                ClassGuid = GuidDevinterfaceUSBDevice,
                Name = 0
            };

            dbi.Size = Marshal.SizeOf(dbi);
            IntPtr buffer = Marshal.AllocHGlobal(dbi.Size);
            Marshal.StructureToPtr(dbi, buffer, true);

            notificationHandle = RegisterDeviceNotification(windowHandle, buffer, 0);
        }

        /// <summary>
        /// Unregisters the window for USB device notifications
        /// </summary>
        public static void UnregisterUsbDeviceNotification()
        {
            UnregisterDeviceNotification(notificationHandle);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, int flags);

        [DllImport("user32.dll")]
        private static extern bool UnregisterDeviceNotification(IntPtr handle);

        [StructLayout(LayoutKind.Sequential)]
        private struct DevBroadcastDeviceinterface
        {
            internal int Size;
            internal int DeviceType;
            internal int Reserved;
            internal Guid ClassGuid;
            internal short Name;
        }

        public static List<string> DisplayingExistPort()
        {
            List<string> ports = new();
            //ManagementObjectSearcher drives = new ManagementObjectSearcher(
            //                                    "SELECT * FROM Win32_USBHub");
            //foreach (var item in drives.Get())
            //{
            //    ports.Add(item.Properties["Description"].Value.ToString());        
            //}

            for (int i = 0; i < SerialPort.GetPortNames().Length; i++)
            {
                ports.Add(SerialPort.GetPortNames()[i]);    
            }
            return ports;
        }
        public bool ConnectPort(string port)
        {
            SettingPortSettigns();
            if (m_comPort.IsOpen == true)
            {
                m_comPort.Close();
            }
            m_comPort.PortName = port;

            try
            {
                m_comPort.Open();
                m_comPort.Write(m_checkRequest, 0, m_checkRequest.Length);
                Thread.Sleep(100);
                string Responce = m_comPort.ReadExisting();
                if (Responce.StartsWith("ForsInjekt"))
                {

                }
                else if (Responce.StartsWith("Injector"))
                {
                    SetPortName(m_comPort.PortName);
                    m_comPort.DataReceived += ReadingResponse;
                    CheckerStart();
                    m_buffer = "";
                    return true;
                }
                else m_comPort.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return m_comPort.IsOpen;
        }
        public string GetPortName()
        {
            return m_currentPortName;
        }
        private void SetPortName(string name)
        {
            m_currentPortName = name;
        }
        private static bool FindingPorts(string port)
        {
            SerialPort tempSerial = new SerialPort();
            tempSerial.BaudRate = m_BaudRate;
            tempSerial.Parity = m_Parity;
            tempSerial.DataBits = m_DataBits;
            tempSerial.StopBits = m_StopBits;
            tempSerial.PortName = port;
            try
            {
                tempSerial.Open();
                tempSerial.Write(m_checkRequest, 0, m_checkRequest.Length);
                Thread.Sleep(100);
                string Responce = tempSerial.ReadExisting();
                if (Responce.StartsWith("Injector") || Responce.StartsWith("ForsInjekt"))
                {
                    tempSerial.Close();
                    return true;
                }
                else
                {
                    tempSerial.Close();
                    return false;
                }

            }
            catch (Exception)
            {

                throw;
            }
        } 
        public bool AutoConnect()
        {
            var allPort = DisplayingExistPort();
            bool finded = false;
            for (int i = 0; i < allPort.Count; i++)
            {
                if (FindingPorts(allPort[i]) == true)
                {
                    SetPortName(allPort[i]);
                    finded = true;
                    break;
                }
            }
            return finded;
        }

        private void SettingPortSettigns()
        {
            m_comPort.BaudRate = m_BaudRate;
            m_comPort.Parity = m_Parity;
            m_comPort.DataBits = m_DataBits;
            m_comPort.StopBits = m_StopBits;
        }

        private void ReadingResponse(object sender, SerialDataReceivedEventArgs e)
        {
            m_buffer += m_comPort.ReadExisting();
            {
                string[] lines = m_buffer.Split('\n');
                m_list = lines;
                int num = m_list.Length -1;
                m_buffer = m_list[num];
                for (int i = 0; i < num; i++)
                {
                    ResponseSorting(m_list[i]);
                }
            }

        }

        private void DisconnectPort()
        {
            m_comPort.DataReceived -= ReadingResponse;
            m_checkConnection.Elapsed -= Callback;
            EventBus.Disconnecting();
            m_comPort.Close();
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
            if (m_comPort.IsOpen == false || m_ping > 2)
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
                m_comPort.Write(request);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        private  float StrToFloat(string line, ref int pos)
        {
            float i = 0;
            char c = ' ';
            float k = 1;
            // пропускаем пробелы, если есть
            while (line[pos] == ' ')
            {
                pos++;
            }
            // цифры до точки
            while (pos < line.Length)
            {
                c = line[pos++];
                if (c >= '0' && c <= '9')
                {
                    i = i * 10 + c - '0';
                }
                else break;
            }
            if (c != '.') return i;
            // цифры после точки
            while (pos < line.Length)
            {
                c = line[pos++];
                if (c >= '0' && c <= '9')
                {
                    k /= 10;
                    i = i + (c - '0') * k;
                }
                else break;
            }
            return i;
        }
        private void ResponseSorting(string line)
        {
            int charnum = 0;
            char data;
            float value;
            // проверка ответов которые занимают ВСЮ строку
            switch (line[0])
            {
                case 'T':   // таблица с данными тестирования форсунок
                    //CalculationMeasurements(line);
                    return;
                case 'E':   // ошибка
                    charnum++;
                    switch (Convert.ToInt32(StrToFloat(line, ref charnum)))
                    {
                        case 1:
                            value = StrToFloat(line, ref charnum);
                            //MessageContent = "Неполадки питания. Напряжение = " + value.ToString("F") + " В";
                            //ChengedMessageContentColor(new SolidColorBrush(Properties.Settings.Default.MessageContentError));
                            //SwitchTab();
                            //ShowMessage("Неполадки питания\nНапряжения = " + value.ToString("F") + " В", "Питание", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
                            break;
                        case 2:
                            //MessageContent = "Высокий ток форсунки";// пергрузка форсунок
                            //ChengedMessageContentColor(new SolidColorBrush(Properties.Settings.Default.MessageContentError));
                            break;
                        case 3:
                            //MessageContent = "Перегрузка насоса.";
                            //ChengedMessageContentColor(new SolidColorBrush(Properties.Settings.Default.MessageContentError));
                            //ShowMessage("Перегрузка насоса.", "Сбой", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
                            break;
                        case 11:
                            //IsAccessAllowed = false;
                            //IsAccessAllowed = false;
                            //MessageContent = "Не подключено ни одной исправной форсунки.";
                            //ChengedMessageContentColor(new SolidColorBrush(Properties.Settings.Default.MessageContentError));
                            //ShowMessage("Не подключено ни одной исправной форсунки.", "Проверка", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
                            break;
                        case 12:
                            value = StrToFloat(line, ref charnum);
                            //IsAccessAllowed = false;
                            //IsAccessAllowed = false;
                            //MessageContent = "Форсунки имеют слишком большую разницу сопротивления";
                            //ChengedMessageContentColor(new SolidColorBrush(Properties.Settings.Default.MessageContentError));
                            //ShowMessage("Форсунки имеют слишком большую разницу сопротивления\nБолее " + value.ToString("F") + " Ом", "Проверка", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
                            break;
                        case 14:
                            //MessageContent = "Выбрано больше одной форсунки";
                            //ChengedMessageContentColor(new SolidColorBrush(Properties.Settings.Default.MessageContentError));
                            //ShowMessage("Выбрано больше одной форсунки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.None);
                            break;
                        case 20:
                            value = StrToFloat(line, ref charnum);
                            //MessageContent = "Калибровка успешно завершена.";
                            //ChengedMessageContentColor(new SolidColorBrush(Properties.Settings.Default.MessageContentDefault));
                            //ShowMessage("Калибровка успешно завершена\nКоэфицент питания = " + value.ToString("F0") + "\nКоэфицент тока = " + StrToFloat(line, ref charnum).ToString("F0"), "Калибровка", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
                            break;
                        case 21:
                            value = StrToFloat(line, ref charnum);
                            //MessageContent = "Возможно, вы указали неверное напряжение. Коэфицент должен быть около 12100";
                            //ChengedMessageContentColor(new SolidColorBrush(Properties.Settings.Default.MessageContentError));
                            //ShowMessage("Возможно, вы указали неверное напряжение\nКоэфицент должен быть около 12100\nПолучен = " + value.ToString("F0"), "Ошибка калибровки", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
                            break;
                        case 22:
                            value = StrToFloat(line, ref charnum);
                            //MessageContent = "Возможно, вы не замкнули выводы первой форсунки .Коэфицент должен быть около 11000";
                            //ChengedMessageContentColor(new SolidColorBrush(Properties.Settings.Default.MessageContentError));
                            //ShowMessage("Возможно, вы не замкнули выводы первой форсунки\nКоэфицент должен быть около 11000\nПолучен = " + value.ToString("F0"), "Ошибка калибровки", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
                            break;
                        default:
                            //MessageContent = line;
                            break;
                    }
                    return;
                case 'F':   // сопротивление форсунок
                    //IsAccessAllowed = true;
                    //MessageContent = "";
                    //MeasureResistance(line);
                    break;
                case 'I':
                    if (line[1] == 'C')
                    {
                        string date = line.Substring(2);
                        //_buildDateFirmware = DateTime.Parse(date);
                        //RaisePropertyChanged("VersionFirmware");

                    }
                    break;
            }
            // переваривание обычных ответов
            while (charnum < line.Length)
            {
                data = line[charnum++];
                switch (data)
                {
                    case ' ':   // пробел пропускаем
                        break;
                    case 'V':   // напряжение питания
                        if (m_ping > 0) m_ping--;
                        //VoltageValue = StrToFloat(line, ref charnum).ToString("F");
                        break;
                    case 'I':   // коментарий, дальше все игнорится
                        return;
                    case 'W':   // битовая маска работающих форсунок
                        //_nozzleBitMask = Convert.ToInt32(StrToFloat(line, ref charnum));
                        //PreSettingColorInteractiveNozzle();
                        break;
                    case 'P':
                        value = StrToFloat(line, ref charnum);
                        //if (_pumpPower != value)
                        //{
                        //    _pumpPower = value;
                        //    RaisePropertyChanged("PumpPower");
                        //}
                        break;
                    case 'X':
                        value = StrToFloat(line, ref charnum);
                        //if (_openningImpulse != value)
                        //{
                        //    _openningImpulse = Math.Round(value, 1);
                        //    RaisePropertyChanged("OpenningImpulse");
                        //}
                        break;
                    case 'Q':
                        value = StrToFloat(line, ref charnum);
                        //if (_retentionFrequency != value)
                        //{
                        //    _retentionFrequency = value;
                        //    RaisePropertyChanged("RetentionFrequency");
                        //}
                        break;
                    case 'R':
                        value = StrToFloat(line, ref charnum);
                        //if (_engineSpeed != value)
                        //{
                        //    _engineSpeed = value;
                        //    RaisePropertyChanged("EngineSpeed");
                        //}
                        break;
                    case 'D':
                        value = StrToFloat(line, ref charnum);
                        //if (_borehole != value)
                        //{
                        //    _borehole = Math.Round(value, 1);
                        //    RaisePropertyChanged("Borehole");
                        //}
                        break;
                    case 'U':
                        //_pressure = StrToFloat(line, ref charnum) * 25;
                        //CurrentPressure = _pressure.ToString("0.#");
                        //if (_pressure > 115)
                        //{
                        //    AllowTestColor = "#FF00CC00";
                        //    if (_selectedNozzleNum != 0) AllowTestButton = true;
                        //}
                        //if (_pressure < 110)
                        //{
                        //    AllowTestColor = "#FF000000";
                        //    AllowTestButton = false;
                        //}
                        break;
                    case 'H':
                        //value = StrToFloat(line, ref charnum);
                        //if (_retentionDuration != value)
                        //{
                        //    _retentionDuration = Math.Round(value, 1);
                        //    RaisePropertyChanged("RetentionDuration");
                        //}
                        break;
                    case 'S':
                        switch (Convert.ToInt32(StrToFloat(line, ref charnum)))
                        {
                            case 6:
                                //_isDrainOn = true;
                                break;
                            case 10:
                                //Indicators[0] = "#FFA0A0A0";
                                break;
                            case 11:
                                //Indicators[0] = "#FF48E013";
                                break;
                            case 20:
                                //Indicators[1] = "#FFA0A0A0";
                                //_isDrainOn = false;
                                break;
                            case 21:
                                //Indicators[1] = "#FF48E013";
                                //_isDrainOn = true;
                                break;
                            case 1:
                            case 4:
                            case 5:
                                //IsAllowStartMode = false;
                                //IsConnectionDone = false;
                                break;
                            case 0:
                                //if (_isCavitationOn == true)
                                //{
                                //    _isCavitationOn = false;
                                //}
                                //timerExecuting.Stop();
                                //timerChangedEngineSeped?.Close();
                                //if ((int)SelectedWorkMode == 8)
                                //{
                                //    timerSwitchPump.Close();
                                //}
                                //if (SelectedWorkMode == WorkMode.SpeedTest)
                                //{
                                //    timerForHPFP.Elapsed -= SmoothChangeSpeed;
                                //    _setEngineSpeed = 0;
                                //    _oldEngineSpeed = 0;
                                //}
                                //SelectedWorkMode = WorkMode.None;
                                //IsAllowStartMode = true;
                                //IsChangedTimeMode = true;
                                //IsConnectionDone = true;
                                ////if (TimeMode < 30) TimeMode = 30;
                                //ProgressValue = 0;
                                //ResetSettings();
                                break;
                        }
                        break;
                }
            }
        }
    }
}
