using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace DalamudProfiles
{
    /// <summary>
    /// Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            loadPaths("SETTINGSpluginpath");
            loadPaths("SETTINGSpluginpath");
            loadPaths("SETTINGSpluginpath");
            loadStuff();
        }

        private void checkKeys(string key)
        {/*
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }*/
        }

        private void loadPaths(string key)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, "Please set path");
                }
                
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        private void loadStuff()
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            lbl_pluginPath.Content = settings["SETTINGSpluginpath"].Value;
            lbl_uninstalledpath.Content = settings["SETTINGSuninstalledpath"].Value;
            lbl_xivlauncherpath.Content = settings["SETTINGSxivlauncherpath"].Value;
        }

        private void btn_changePluginPath_Click(object sender, RoutedEventArgs e)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };
            dialog.ShowDialog();

            try
            {
                settings["SETTINGSpluginpath"].Value = dialog.FileName;

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }catch (InvalidOperationException)
            {
                Console.WriteLine("Error xd");
            }
            
            loadStuff();
        }
    }
}
