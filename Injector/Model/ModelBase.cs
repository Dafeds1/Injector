using Injector.ConnectionToMC;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
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
        public List<String> AllPorts
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

        private static ObservableCollection<string> m_nozzlesResistance = new() { "-1", "-1", "-1", "-1", "-1", "-1" };
        public ObservableCollection<string> NozzlesResistance
        {
            get { return m_nozzlesResistance; }
            set { m_nozzlesResistance = value; RaisePropertyChanged("NozzlesResistance"); }
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
                    m_timeMode = value;
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
                    m_pumpPower = Math.Round(value,0);
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
                    m_engineSpeed = Math.Round(value,0);
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
        private string m_voltageValue = "0,00";
        public string VoltageValue
        {
            get { return m_voltageValue; }
            set { m_voltageValue = value; RaisePropertyChanged("VoltageValue"); }
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



        public void FindAllPorts()
        {
            AllPorts = Connection.DisplayingExistPort();
        }
        public void ConnectTo(string port)
        {
            if (m_connection.ConnectPort(port) == true)
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
                SubscribeOnDisconnect(new ObservableCollection<string> { "5","5", "5", "5", "5", "5"});
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
        }

        public void SendCommand(string command)
        {
            m_connection.SendRequest(command);
        }

        public void RefreshListPorts()
        {
            AllPorts = Connection.DisplayingExistPort();
        }
    }
}
