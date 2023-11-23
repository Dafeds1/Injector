using Injector.Model;
using Prism.Commands;
using System;
using System.Windows;

namespace Injector.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public DelegateCommand<object> SwitchContent { get; }

        private ViewModelBase _currentChildView;
        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                RaisePropertyChanged(nameof(CurrentChildView));
            }
        }

        private void SwitchContentTo(object index)
        {
            try
            {
                int contentIndex = Convert.ToInt32(index);
                switch (contentIndex)
                {
                    case 0:
                        CurrentChildView = new HomeViewModel();
                        break;
                    case 1:
                        CurrentChildView = new TestViewModel();
                        break;
                    case 2:
                        CurrentChildView = new CavitationViewModel();
                        break;
                    case 3:
                        CurrentChildView = new AdditionallyViewModel();
                        break;
                    case 4:
                        CurrentChildView = new ReferenceViewModel();
                        break;
                    case 5:
                        CurrentChildView = new SettingsViewModel();
                        break;
                    case 6:
                        CurrentChildView = new SettingsViewModel();
                        break;
                    case 7:
                        CurrentChildView = new SettingsViewModel();
                        break;
                }
                CurrentModelBase.UpdateCaption(contentIndex);
            }
            catch (Exception ex)
            {
                throw ex.GetRootException();
            }
        }
        public MainViewModel()
        {
            CurrentChildView = new HomeViewModel();
            SwitchContent = new DelegateCommand<object>(SwitchContentTo);
            EventBus.onDisconnectController += Disconnect;
            CurrentModelBase.Init();
            CurrentModelBase.UpdateCaption(0);
        }
        private void Disconnect()
        {
            CurrentChildView = new HomeViewModel();
            EventBus.onDisconnectController -= Disconnect;
        }
    }
}
