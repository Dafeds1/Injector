using Injector.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Injector.ViewModels
{
    public class ViewModelBase : BindableBase
    {
        private ModelBase _model = new ModelBase();
        public ModelBase CurrentModelBase
        {
            get { return _model; }
            set { _model = value; RaisePropertyChanged("CurrentModelBase"); }
        }
        public ViewModelBase()
        {
            //_model.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
        }

    }
}
