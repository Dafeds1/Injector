using Injector.ConnectionToMC;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Injector.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public DelegateCommand AutoConnect { get; }
        public DelegateCommand<string> ConnectTo { get; }
        public DelegateCommand<string> SendRequest { get; }
        public DelegateCommand RefreshListPort { get; }
        public HomeViewModel()
        {
            CurrentModelBase.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };        
            PortsSearch();
            AutoConnect = new DelegateCommand(CurrentModelBase.AutoConnect);
            ConnectTo = new DelegateCommand<string>(ConnectToSelectedPort);
            SendRequest = new DelegateCommand<string>(Send);
            RefreshListPort = new DelegateCommand(Refresh);
        }

        private void PortsSearch()
        {
            CurrentModelBase.FindAllPorts();
        }

        private void ConnectToSelectedPort(string portName)
        {
            CurrentModelBase.ConnectTo(portName);
        }
        private void Send(string command)
        {
            CurrentModelBase.SendCommand(command);
        }
        private void Refresh()
        {
            CurrentModelBase.RefreshListPorts();
        }

    }
}
