using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Injector.ViewModels
{
    public class TestViewModel : ViewModelBase
    {
        //public DelegateCommand<string> SendRequest { get; }
        public TestViewModel()
        {
            //SendRequest = new DelegateCommand<string>(Send);

        }
        private void Send(string command)
        {
            //CurrentModelBase.SendCommand(command);
        }   
    }
}
