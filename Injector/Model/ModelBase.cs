using Injector.ConnectionToMC;
using Injector.Views;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Injector.Model
{
    public class ModelBase : BindableBase
    {
        private static Connection m_connection = new Connection();

        private List<string> m_portsList;
        public List<string> AllPorts
        {
            get { return m_portsList; }
            set
            {
                m_portsList = value;
                RaisePropertyChanged(nameof(AllPorts));
            }
        }
        private static string m_selectedPort;
        public string SelectedPort
        {
            get { return m_selectedPort; }
            set
            {
                m_selectedPort = value;
                RaisePropertyChanged(nameof(SelectedPort));
            }
        }
        private static ObservableCollection<string> m_injectorsResistance = new() { "-1", "-1", "-1", "-1", "-1", "-1" };
        public ObservableCollection<string> InjectorsResistance
        {
            get { return m_injectorsResistance; }
            set
            {
                m_injectorsResistance = value;
                RaisePropertyChanged(nameof(InjectorsResistance));
            }
        }
        private readonly ObservableCollection<Uri> _controlIcons = new()
        {
            new Uri(Environment.CurrentDirectory + "/Assets/close.png"),
            new Uri(Environment.CurrentDirectory + "/Assets/maximize.png"),
            new Uri(Environment.CurrentDirectory + "/Assets/minimize.png")};

        public ObservableCollection<Uri> ControlIcons
        {
            get { return _controlIcons; }
        }

        private static ObservableCollection<Uri> m_nozzleState = new()
        {
            new Uri(Environment.CurrentDirectory + "/Images/Nozzle.png"),
            new Uri(Environment.CurrentDirectory + "/Images/Nozzle.png"),
            new Uri(Environment.CurrentDirectory + "/Images/Nozzle.png"),
            new Uri(Environment.CurrentDirectory + "/Images/Nozzle.png"),
            new Uri(Environment.CurrentDirectory + "/Images/Nozzle.png"),
            new Uri(Environment.CurrentDirectory + "/Images/Nozzle.png")
        };
        public ObservableCollection<Uri> NozzleState
        {
            get { return m_nozzleState; }
            set { m_nozzleState = value; RaisePropertyChanged(nameof(NozzleState)); }
        }
        private static ObservableCollection<Uri> m_colorInteractiveNozzle = new()
        {
            new Uri(Environment.CurrentDirectory + "/Images/Nozzle.png"),
            new Uri(Environment.CurrentDirectory + "/Images/Nozzle.png"),
            new Uri(Environment.CurrentDirectory + "/Images/Nozzle.png"),
            new Uri(Environment.CurrentDirectory + "/Images/Nozzle.png"),
            new Uri(Environment.CurrentDirectory + "/Images/Nozzle.png"),
            new Uri(Environment.CurrentDirectory + "/Images/Nozzle.png")

        };
        public ObservableCollection<Uri> ColorInteractiveNozzle
        {
            get { return m_colorInteractiveNozzle; }
            set
            {
                m_colorInteractiveNozzle = value;
                RaisePropertyChanged(nameof(ColorInteractiveNozzle));
            }
        }


        private ObservableCollection<string> m_reportResistance = new() { "-1", "-1", "-1", "-1", "-1", "-1" };
        public ObservableCollection<string> ReportResistance
        {
            get { return m_reportResistance; }
            set
            {
                m_reportResistance = value;
                RaisePropertyChanged(nameof(ReportResistance));
            }
        }
        private static ObservableCollection<string> m_averageReaction = new() { "0 мс", "0 мс", "0 мс", "0 мс", "0 мс", "0 мс" };
        public ObservableCollection<string> AverageReaction
        {
            get { return m_averageReaction; }
            set { m_averageReaction = value; RaisePropertyChanged("AverageReaction"); }
        }
        private static ObservableCollection<string> m_averageOpenning = new() { "0 мс", "0 мс", "0 мс", "0 мс", "0 мс", "0 мс" };
        public ObservableCollection<string> AverageOpenning
        {
            get { return m_averageOpenning; }
            set { m_averageOpenning = value; RaisePropertyChanged("AverageOpenning"); }
        }
        private static ObservableCollection<string> m_averageSaturation = new() { "0 A", "0 A", "0 A", "0 A", "0 A", "0 A" };
        public ObservableCollection<string> AverageSaturation
        {
            get { return m_averageSaturation; }
            set { m_averageSaturation = value; RaisePropertyChanged("AverageSaturation"); }
        }
        private static ObservableCollection<string> m_delta = new() { "0 мс", "0 мс", "0 мс", "0 мс", "0 мс", "0 мс" };
        public ObservableCollection<string> Delta
        {
            get { return m_delta; }
            set { m_delta = value; RaisePropertyChanged("Delta"); }
        }

        private ObservableCollection<Uri> m_interactiveNozzle = new() {
            new Uri(Environment.CurrentDirectory + "/Images/Nozzle.png"),
            new Uri(Environment.CurrentDirectory + "/Images/Nozzle.png"),
            new Uri(Environment.CurrentDirectory + "/Images/Nozzle.png"),
            new Uri(Environment.CurrentDirectory + "/Images/Nozzle.png"),
            new Uri(Environment.CurrentDirectory + "/Images/Nozzle.png"),
            new Uri(Environment.CurrentDirectory + "/Images/Nozzle.png")
        };
        public ObservableCollection<Uri> InteractiveNozzle
        {
            get { return m_interactiveNozzle; }
            set
            {
                m_interactiveNozzle = value;
                RaisePropertyChanged(nameof(InteractiveNozzle));
            }
        }

        #region Testing parametrs

        private double m_timeMode = 120;
        public double TimeMode
        {
            get
            {
                return m_timeMode;
            }
            set
            {
                if (m_timeMode != value)
                {
                    m_timeMode = Math.Round(value, 0);
                    RaisePropertyChanged("TimeMode");
                }
            }
        }
        private double m_cavitationFrequency = 420;
        public double CavitationFrequency
        {
            get { return m_cavitationFrequency; }
            set
            {
                if (m_cavitationFrequency != value)
                {
                    m_cavitationFrequency = Math.Round(value, 1);
                    RaisePropertyChanged("CavitationFrequency");
                    SendCommand($"Q{m_cavitationFrequency.ToString("0.#", CultureInfo.InvariantCulture)}");
                }
            }
        }
        private double m_cavitationBorehole = 50;
        public double CavitationBorehole
        {
            get { return m_cavitationBorehole; }
            set
            {
                if (m_cavitationBorehole != value)
                {
                    m_cavitationBorehole = Math.Round(value, 1);
                    RaisePropertyChanged("CavitationBorehole");
                    SendCommand($"D{m_cavitationBorehole.ToString("0.#", CultureInfo.InvariantCulture)}");
                }
            }
        }
        private double m_pumpPower = 80;
        public double PumpPower
        {
            get
            {
                return m_pumpPower;
            }
            set
            {
                if (m_pumpPower != value)
                {
                    m_pumpPower = Math.Round(value, 0);
                    RaisePropertyChanged("PumpPower");
                    SendCommand($"P{m_pumpPower}");
                }
            }
        }
        private double m_openningImpulse = 1.5;
        public double OpenningImpulse
        {
            get { return m_openningImpulse; }
            set
            {
                if (m_openningImpulse != value)
                {
                    m_openningImpulse = Math.Round(value, 1);
                    RaisePropertyChanged("OpenningImpulse");
                    SendCommand($"X{value.ToString("0.###", CultureInfo.InvariantCulture)}");
                }
            }
        }
        private double m_retentionFrequency = 6000;
        public double RetentionFrequency
        {
            get { return m_retentionFrequency; }
            set
            {
                if (m_retentionFrequency != value)
                {
                    m_retentionFrequency = Math.Round(value, 0);
                    RaisePropertyChanged("RetentionFrequency");
                    SendCommand($"Q{m_retentionFrequency}");
                }
            }
        }
        private double m_engineSpeed = 1000;
        public double EngineSpeed
        {
            get { return m_engineSpeed; }
            set
            {
                if (m_engineSpeed != value)
                {
                    m_engineSpeed = Math.Round(value, 0);
                    RaisePropertyChanged("EngineSpeed");
                    SendCommand($"R{m_engineSpeed}");
                }
            }
        }
        private double m_borehole = 70;
        public double Borehole
        {
            get { return m_borehole; }
            set
            {
                if (m_borehole != value)
                {
                    m_borehole = Math.Round(value, 0);
                    RaisePropertyChanged("Borehole");
                    SendCommand($"D{m_borehole}");
                }
            }
        }
        private double m_retentionDuration = 1;
        public double RetentionDuration
        {
            get { return m_retentionDuration; }
            set
            {
                if (m_retentionDuration != value)
                {
                    m_retentionDuration = Math.Round(value, 1);
                    RaisePropertyChanged("RetentionDuration");
                    SendCommand($"H{m_retentionDuration.ToString("0.###", CultureInfo.InvariantCulture)}");
                }
            }
        }
        private double m_timeStartup = 3;
        public double TimeStartup
        {
            get { return m_timeStartup; }
            set
            {
                if (m_timeStartup != value)
                {
                    m_timeStartup = value;
                    RaisePropertyChanged("TimeStartup");
                }
            }
        }
        private double m_timeShutdown = 5;
        public double TimeShutdown
        {
            get { return m_timeShutdown; }
            set
            {
                if (m_timeShutdown != value)
                {
                    m_timeShutdown = value;
                    RaisePropertyChanged("TimeShutdown");
                }
            }
        }

        #endregion

        private string m_voltageValue = "0,00";
        public string VoltageValue
        {
            get { return m_voltageValue; }
            set { m_voltageValue = value; RaisePropertyChanged("VoltageValue"); }
        }


        #region Application Settings

        private bool m_isSubscribeActvie = Properties.Settings.Default.Subscribe;
        public bool IsSubscribeActive
        {
            get { return m_isSubscribeActvie; }
            set { m_isSubscribeActvie = value; RaisePropertyChanged(nameof(IsSubscribeActive)); }
        }
        private ObservableCollection<string> m_captions = new ObservableCollection<string>();
        private string m_selectedCaption;
        public string SelectedCaption 
        {
            get { return m_selectedCaption; }
            set { m_selectedCaption = value; RaisePropertyChanged(nameof(SelectedCaption)); }
        }
        private ObservableCollection<Uri> m_icons = new ObservableCollection<Uri>()
        {
            new Uri(Environment.CurrentDirectory + "/Assets/Home.png"),
            new Uri(Environment.CurrentDirectory + "/Assets/Testing.png"),
            new Uri(Environment.CurrentDirectory + "/Assets/Testing.png"),
            new Uri(Environment.CurrentDirectory + "/Assets/Additionally.png"),
            new Uri(Environment.CurrentDirectory + "/Assets/Reference.png"),
            new Uri(Environment.CurrentDirectory + "/Assets/DBInjectors.png"),
            new Uri(Environment.CurrentDirectory + "/Assets/AutoDiagnostic.png"),
            new Uri(Environment.CurrentDirectory + "/Assets/Settings.png")
        };
        public ObservableCollection<Uri> Icons
        {
            get { return m_icons; }
            set { m_icons = value; RaisePropertyChanged(nameof(Icons)); }
        }
        private ObservableCollection<string> m_listLanguage = new()
        {
            AppLanguages.GetAllLanguages(0).NativeName,
            AppLanguages.GetAllLanguages(1).NativeName,
            AppLanguages.GetAllLanguages(2).NativeName,
        };
        public ObservableCollection<string> ListLanguage
        {
            get { return m_listLanguage; }
            set { m_listLanguage = value; RaisePropertyChanged(nameof(ListLanguage)); }
        }

        private string m_selectedLanguage = Properties.Settings.Default.DefaultLanguage.NativeName;
        public string SelectedLanguage
        {
            get { return m_selectedLanguage; }
            set
            {
                m_selectedLanguage = value;
                RaisePropertyChanged(nameof(SelectedLanguage));
            }
        }

        private ObservableCollection<string> m_listThemes = AppTheme.GetAllThemes();
        public ObservableCollection<string> ListThemes
        {
            get { return m_listThemes; }
            set { m_listThemes = value; RaisePropertyChanged(nameof(ListThemes)); }
        }

        private string m_selectedTheme = AppTheme.GetSelectedThemes();
        public string SelectedTheme
        {
            get { return m_selectedTheme; }
            set { m_selectedTheme = value; RaisePropertyChanged(nameof(SelectedTheme)); }
        }
        private string m_logsDirectory = Properties.Settings.Default.LogsDirectory;
        public string LogsDirectory
        {
            get {return m_logsDirectory; }
            set { m_logsDirectory = value; RaisePropertyChanged(nameof(LogsDirectory)); }
        }
        #endregion


        private bool m_isConnected = false;
        public bool IsConnected
        {
            get { return m_isConnected; }
            set { m_isConnected = value; RaisePropertyChanged(nameof(IsConnected)); }
        }


        public void Init()
        {
            
            ResourceDictionary resourceDictionary = new ResourceDictionary() { Source = new Uri(AppLanguages.GetSelectedLanguage(), UriKind.Relative) };
            m_captions.Add((string)resourceDictionary["m_homeCaption"]);
            m_captions.Add((string)resourceDictionary["m_testingCaption"]);
            m_captions.Add((string)resourceDictionary["m_cavitationCaption"]);
            m_captions.Add((string)resourceDictionary["m_additionallyCaption"]);
            m_captions.Add((string)resourceDictionary["m_referenceCaption"]);
            m_captions.Add((string)resourceDictionary["m_databaseCaption"]);
            m_captions.Add((string)resourceDictionary["m_autoDiagnosticCaption"]);
            m_captions.Add((string)resourceDictionary["m_settingsCaption"]);
            m_captions.Add((string)resourceDictionary["m_calibrateCaption"]);
            m_captions.Add((string)resourceDictionary["m_updatesCaption"]);
            if (Properties.Settings.Default.Subscribe == false)
            {
                Icons[5] = new Uri(Environment.CurrentDirectory + "/Assets/Lock.png");
                Icons[6] = new Uri(Environment.CurrentDirectory + "/Assets/Lock.png");
            }
        }
        public void UpdateCaption(int index)
        {
            SelectedCaption = m_captions[index];    
        }
        public void FindAllPorts()
        {
            AllPorts = Connection.DisplayingExistDevices();
        }
        public void ConnectTo(string port)
        {
            if (port != null)
            {
                m_selectedPort = m_connection.GetPortName();
                RaisePropertyChanged(nameof(SelectedPort));
                SubscribeOnDisconnect(new ObservableCollection<string> { "5", "5", "5", "5", "5", "5" });
                //MessageBox.Show("Connect");
            }

        }
        public void AutoConnect()
        {
            RefreshListPorts();
            if (m_connection.AutoConnect() == true)
            {
                m_selectedPort = m_connection.GetPortName();
                RaisePropertyChanged(nameof(SelectedPort));
                SubscribeOnDisconnect(new ObservableCollection<string> { "5", "5", "5", "5", "5", "5" });
                EventBus.onReceiveResponse += ResponseSorting;
            }
        }
        private void SubscribeOnDisconnect(ObservableCollection<string> meas)
        {
            EventBus.onDisconnectController += Disconnect;
            InjectorsResistance = meas;
        }
        private void Disconnect()
        {
            InjectorsResistance = new ObservableCollection<string> { "1", "1", "1", "1", "1", "1" };
            EventBus.onDisconnectController -= Disconnect;
            EventBus.onReceiveResponse -= ResponseSorting;
        }

        public void SendCommand(string command)
        {
            m_connection.SendRequest(command);
        }

        public void RefreshListPorts()
        {
            AllPorts = Connection.DisplayingExistDevices();
            RaisePropertyChanged(nameof(AllPorts));
        }

        private float StrToFloat(string line, ref int pos)
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
                    CalculationMeasurements(line);
                    return;
                case 'E':   // ошибка
                    charnum++;
                    switch (Convert.ToInt32(StrToFloat(line, ref charnum)))
                    {
                        case 1:
                            value = StrToFloat(line, ref charnum);
                            //MessageContent = "Неполадки питания. Напряжение = " + value.ToString("F") + " В";
                            ShowMessage("Неполадки питания\nНапряжения = " + value.ToString("F") + " В", "Питание", MessageBoxButton.OK);
                            break;
                        case 2:
                            //MessageContent = "Высокий ток форсунки";// пергрузка форсунок
                            break;
                        case 3:
                            //MessageContent = "Перегрузка насоса.";
                            //ChengedMessageContentColor(new SolidColorBrush(Properties.Settings.Default.MessageContentError));
                            ShowMessage("Перегрузка насоса.", "Сбой", MessageBoxButton.OK);
                            break;
                        case 11:
                            //IsAccessAllowed = false;
                            //IsAccessAllowed = false;
                            //MessageContent = "Не подключено ни одной исправной форсунки.";
                            //ChengedMessageContentColor(new SolidColorBrush(Properties.Settings.Default.MessageContentError));
                            ShowMessage("Не подключено ни одной исправной форсунки.", "Проверка", MessageBoxButton.OK);
                            break;
                        case 12:
                            value = StrToFloat(line, ref charnum);
                            //IsAccessAllowed = false;
                            //IsAccessAllowed = false;
                            //MessageContent = "Форсунки имеют слишком большую разницу сопротивления";
                            ShowMessage("Форсунки имеют слишком большую разницу сопротивления\nБолее " + value.ToString("F") + " Ом", "Проверка", MessageBoxButton.OK);
                            break;
                        case 14:
                            //MessageContent = "Выбрано больше одной форсунки";
                            ShowMessage("Выбрано больше одной форсунки", "Ошибка", MessageBoxButton.OK);
                            break;
                        case 20:
                            value = StrToFloat(line, ref charnum);
                            //MessageContent = "Калибровка успешно завершена.";
                            ShowMessage("Калибровка успешно завершена\nКоэфицент питания = " + value.ToString("F0") + "\nКоэфицент тока = " + StrToFloat(line, ref charnum).ToString("F0"), "Калибровка", MessageBoxButton.OK);
                            break;
                        case 21:
                            value = StrToFloat(line, ref charnum);
                            //MessageContent = "Возможно, вы указали неверное напряжение. Коэфицент должен быть около 12100";
                            ShowMessage("Возможно, вы указали неверное напряжение\nКоэфицент должен быть около 12100\nПолучен = " + value.ToString("F0"), "Ошибка калибровки", MessageBoxButton.OK);
                            break;
                        case 22:
                            value = StrToFloat(line, ref charnum);
                            //MessageContent = "Возможно, вы не замкнули выводы первой форсунки .Коэфицент должен быть около 11000";;
                            ShowMessage("Возможно, вы не замкнули выводы первой форсунки\nКоэфицент должен быть около 11000\nПолучен = " + value.ToString("F0"), "Ошибка калибровки", MessageBoxButton.OK);
                            break;
                        default:
                            //MessageContent = line;
                            break;
                    }
                    
                    return;
                case 'F':   // сопротивление форсунок
                    //IsAccessAllowed = true;
                    //MessageContent = "";
                    MeasureResistance(line);
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
                        VoltageValue = StrToFloat(line, ref charnum).ToString("F");
                        break;
                    case 'I':   // коментарий, дальше все игнорится
                        return;
                    case 'W':   // битовая маска работающих форсунок
                        //_nozzleBitMask = Convert.ToInt32(StrToFloat(line, ref charnum));
                        //PreSettingColorInteractiveNozzle();
                        break;
                    case 'P':
                        value = StrToFloat(line, ref charnum);
                        if (m_pumpPower != value)
                        {
                            m_pumpPower = value;
                            RaisePropertyChanged(nameof(PumpPower));
                        }
                        break;
                    case 'X':
                        value = StrToFloat(line, ref charnum);
                        if (m_openningImpulse != value)
                        {
                            m_openningImpulse = Math.Round(value, 1);
                            RaisePropertyChanged(nameof(OpenningImpulse));
                        }
                        break;
                    case 'Q':
                        value = StrToFloat(line, ref charnum);
                        if (m_retentionFrequency != value)
                        {
                            m_retentionFrequency = value;
                            RaisePropertyChanged(nameof(RetentionFrequency));
                        }
                        break;
                    case 'R':
                        value = StrToFloat(line, ref charnum);
                        if (m_engineSpeed != value)
                        {
                            m_engineSpeed = value;
                            RaisePropertyChanged(nameof(EngineSpeed));
                        }
                        break;
                    case 'D':
                        value = StrToFloat(line, ref charnum);
                        if (m_borehole != value)
                        {
                            m_borehole = Math.Round(value, 1);
                            RaisePropertyChanged(nameof(Borehole));
                        }
                        break;
                    case 'H':
                        value = StrToFloat(line, ref charnum);
                        if (m_retentionDuration != value)
                        {
                            m_retentionDuration = Math.Round(value, 1);
                            RaisePropertyChanged(nameof(RetentionDuration));
                        }
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

        [STAThread]
        private bool? ShowMessage(string message, string caption, MessageBoxButton buttons)
        {
            bool? result = null;
            Application.Current.Dispatcher.BeginInvoke( bool? () =>
            {
                MessageWindow messageWindow = new MessageWindow(message, caption, buttons);
                messageWindow.ShowDialog();
                result = messageWindow.DialogResult;
                return result;
            });
            return result;
        }

        public void MeasureResistance(string mesurements)
        {
            double[] tempR = new double[6];
            string[] lines = mesurements.Split(' ');
            if (lines.Length >= 7)
            {
                NozzleState.Clear();
                InjectorsResistance.Clear();

                for (int n = 0; n < 6; n++)
                {
                    tempR[n] = Math.Round(Convert.ToDouble(lines[n + 1], CultureInfo.InvariantCulture), 1);

                    

                    if (tempR[n] == -2 || (tempR[n] == -3))
                    {
                        NozzleState.Add(new Uri("/Images/NozzleGray.png", UriKind.Relative));
                        InjectorsResistance.Add("--");
                    }
                    else
                    {
                        if ((tempR[n] == -1))
                        {
                            NozzleState.Add(new Uri("/Images/NozzleRed.png", UriKind.Relative));
                            InjectorsResistance.Add("K3");
                        }
                        else
                        {
                            InjectorsResistance.Add(tempR[n].ToString("F1"));
                        }
                    }
                    if (tempR[n] >= 0.1 && tempR[n] < 7)
                    {
                        tempR[n] = tempR[n];
                        NozzleState.Add(new Uri("/Images/NozzleGreen.png", UriKind.Relative));
                    }
                    if (tempR[n] >= 7 && tempR[n] <= 25)
                    {
                        tempR[n] = tempR[n];
                        NozzleState.Add( new Uri("/Images/NozzleBlue.png", UriKind.Relative));
                    }
                }
            }
        }
        public void SaveLogsDirectory(string directory)
        {
            if (Properties.Settings.Default.LogsDirectory == Environment.GetEnvironmentVariable("Temp") + "/InjectorTemp/")
            {
                Directory.Delete(Properties.Settings.Default.LogsDirectory, true);
            }
            LogsDirectory = directory;
            Properties.Settings.Default.LogsDirectory = directory;
            Properties.Settings.Default.Save();


        }
        public void CalculationMeasurements(string line)
        {
            if (line.StartsWith("TN"))
            {
                string numMesurement = line.ElementAt(2).ToString();
                //_numbMeasurements = Convert.ToInt32(numMesurement);
            }
            if (line.StartsWith("TF"))
            {
                string messurement = line.ElementAt(2).ToString();
                int numNozzle = Convert.ToInt32(messurement);
                string[] valuesLine = line.Split(' ');
                for (int v = 1; v < valuesLine.Length; v++)
                {
                    //_totalValues[_numbMeasurements, numNozzle, v - 1] = Convert.ToDouble(valuesLine[v], CultureInfo.InvariantCulture);
                }
            }
            if (line.StartsWith("TE"))
            {
                //DrawingCharts();
                //AverageValues();
            }
        }
    }
}
