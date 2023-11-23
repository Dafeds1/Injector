using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Injector
{
    public class AppLanguages
    {
        private static List<CultureInfo> m_languages = new() { new CultureInfo("ru-RU"), new CultureInfo("en-US"), new CultureInfo("kk-KZ") };
        public List<CultureInfo> Languages
        {
            get { return m_languages; }
        }
        public AppLanguages()
        {

        }

        public static void ChangeApplicationLanguage(int index)
        {
            ResourceDictionary otherResource = Application.Current.Resources.MergedDictionaries.Last();
            Application.Current.Resources.Clear();
            
            switch(index){
                case 0:
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Languages/lang_ru_RU.xaml", UriKind.Relative) });
                    break;
                case 1:
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Languages/lang_en_EN.xaml", UriKind.Relative) });
                    break;
                case 2:
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Languages/lang_kz_KZ.xaml", UriKind.Relative) });
                    break;
                }
            Application.Current.Resources.MergedDictionaries.Add(otherResource);

            Properties.Settings.Default.DefaultLanguage = m_languages[index];
            Properties.Settings.Default.Save();
        }

        public static void LoadDefaultLanguage()
        {
            //MessageBox.Show(lang.Name + '\n' + lang.NativeName + '\n' + lang.Parent + '\n' + lang.DisplayName);
            for (int i = 0; i < m_languages.Count; i++)
            {
                if (m_languages[i].Name == Properties.Settings.Default.DefaultLanguage.Name)
                {
                    ChangeApplicationLanguage(i);
                }
            }
        }
        public static CultureInfo GetCurrentLanguage()
        {
            return Properties.Settings.Default.DefaultLanguage;
        }
        public static CultureInfo GetAllLanguages(int index)
        {
            return m_languages[index];
        }
        public static string GetSelectedLanguage()
        {
            string findedUri = null;
            switch (GetCurrentLanguage().Name)
            {
                case "ru-RU":
                    findedUri = "Languages/lang_ru_RU.xaml";
                    break;
                case "en-US":
                    findedUri = "Languages/lang_en_EN.xaml";
                    break;
                case "kk-KZ":
                    findedUri = "Languages/lang_kz_KZ.xaml";
                    break;
            }
            return findedUri;
        }
    }
}
