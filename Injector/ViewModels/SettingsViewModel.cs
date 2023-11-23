using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Injector.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public DelegateCommand OpenFolder { get; }
        public DelegateCommand FilesBrowse { get; }
        private string m_fileName;
        public SettingsViewModel()
        {
            CurrentModelBase.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            OpenFolder = new DelegateCommand(OpenLogsFolder);
            FilesBrowse = new DelegateCommand(SelectFile);
        }

        private void OpenLogsFolder()
        {
            Process.Start(@Properties.Settings.Default.pathToLogs);
        }
        private void SelectFile()
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = ".txt|*";
            //file.FileOk += Select;
            file.ShowDialog();
            m_fileName = file.FileName;
            Select();
            
        }

        private void Select()
        {
            CurrentModelBase.SaveLogsDirectory(m_fileName);
        }

    }
}
