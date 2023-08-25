using Prism.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Injector
{
    public class AppTheme
    {
        private enum m_themes{
            LightTheme,
            DarkTheme,
            YellowTheme
        }
        public static void ChangeTheme(int index)
        {

            ResourceDictionary otherResource = Application.Current.Resources.MergedDictionaries.First();
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(otherResource);

            switch (index)
            {
                case 0:
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri($"Themes/{(m_themes)index}.xaml", UriKind.Relative) });
                    break;
                case 1:
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri($"Themes/{(m_themes)index}.xaml", UriKind.Relative) });
                    break;
                case 2:
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri($"Themes/{(m_themes)index}.xaml", UriKind.Relative) });
                    break;
            }

            Properties.Settings.Default.DefaultTheme = index;
            Properties.Settings.Default.Save();
        }
        public static void LoadDefaltTheme()
        {
            ChangeTheme(Properties.Settings.Default.DefaultTheme);
        }
        public static ObservableCollection<string> GetAllThemes()
        {
            ObservableCollection<string> themes = new ObservableCollection<string>();
            ResourceDictionary resourceDictionary = new ResourceDictionary() { Source = new Uri(AppLanguages.GetSelectedLanguage(), UriKind.Relative) };
            themes.Add((string)resourceDictionary["m_lightTheme"]);
            themes.Add((string)resourceDictionary["m_darkTheme"]);
            themes.Add((string)resourceDictionary["m_yellowTheme"]);
            return themes;
        }
        public static string GetSelectedThemes()
        {
            string theme = string.Empty;
            ResourceDictionary resourceDictionary = new ResourceDictionary() { Source = new Uri(AppLanguages.GetSelectedLanguage(), UriKind.Relative) };
            switch (Properties.Settings.Default.DefaultTheme)
            {
                case 0:
                    theme = (string)resourceDictionary["m_lightTheme"];
                    break;
                case 1:
                    theme = (string)resourceDictionary["m_darkTheme"];
                    break;
                case 2:
                    theme = (string)resourceDictionary["m_yellowTheme"];
                    break;
            }
            return theme;
        }
    }
}
