using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Injector.ViewModels
{
    public class CavitationViewModel : ViewModelBase
    {
        public CavitationViewModel()
        {
            CurrentModelBase.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
        }
    }
}
