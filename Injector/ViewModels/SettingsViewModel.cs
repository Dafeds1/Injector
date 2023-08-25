using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Injector.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            CurrentModelBase.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
        }
    }
}
