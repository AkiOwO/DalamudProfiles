using System;
using System.Windows;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using ModernWpf;
using System.Windows.Media;
using System.Threading;

namespace DalamudProfiles
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string pluginPath;
        private string uninstalledPluginPath;
        private string xivLauncherPath;

        private static Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private static KeyValueConfigurationCollection settings = configFile.AppSettings.Settings;

        public MainWindow()
        {
            InitializeComponent();
            checkFolders();
            checkPaths("SETTINGSuninstalledpath");
            checkPaths("SETTINGSpluginpath");
            checkPaths("SETTINGSxivlauncherpath");
            checkPaths("lel");
            getAllProfiles();
            loadPlugins();
        }

        public void moveStuff()
        {
            var test = installedList.SelectedItems;
        }

        public void checkFolders()
        {
            string userName = Environment.UserName;
            string path = @"C:\Users\" + userName + @"\AppData\Roaming\XIVLauncher\uninstalledPlugins\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                //Thread.Sleep(2000);
            }
        }

        public void checkPaths(string key)
        {
            string userName = Environment.UserName;

            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null && key == "SETTINGSpluginpath")
                {
                    settings.Add(key, @"C:\Users\" + userName + @"\AppData\Roaming\XIVLauncher\installedPlugins\");
                }
                else if (settings[key] == null && key == "SETTINGSuninstalledpath")
                {
                    settings.Add(key, @"C:\Users\" + userName + @"\AppData\Roaming\XIVLauncher\uninstalledPlugins\");
                }
                else if (settings[key] == null && key == "SETTINGSxivlauncherpath")
                {
                    settings.Add(key, @"C:\Users\" + userName + @"\AppData\Local\XIVLauncher\XIVLauncher.exe");
                }
                else if (settings["SETTINGSPluginpath"] != null && settings["SETTINGSuninstalledpath"] != null && settings["SETTINGSxivlauncherpath"] != null)
                {
                    pluginPath = settings["SETTINGSpluginpath"].Value;
                    uninstalledPluginPath = settings["SETTINGSuninstalledpath"].Value;
                    xivLauncherPath = settings["SETTINGSxivlauncherpath"].Value;
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        public void loadPlugins()
        {
            installedList.Items.Clear();
            uninstalledList.Items.Clear();

            foreach (var path in Directory.GetDirectories(pluginPath))
            {
                var filename = Path.GetFileName(path);
                installedList.Items.Add(filename);
            }

            foreach (var path in Directory.GetDirectories(uninstalledPluginPath))
            {
                var filename = Path.GetFileName(path);
                uninstalledList.Items.Add(filename);
            }
        }

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            loadPlugins();
        }

        private void btn_deinstall_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = installedList.SelectedItems;
            foreach (var filename in selectedItems)
            {
                var sourcePath = Path.Combine(pluginPath, (string)filename);
                var destPath = Path.Combine(uninstalledPluginPath, (string)filename);
                Directory.Move(sourcePath, destPath);
            }
            loadPlugins();
        }

        private void btn_install_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = uninstalledList.SelectedItems;
            foreach (var filename in selectedItems)
            {
                var sourcePath = Path.Combine(uninstalledPluginPath, (string)filename);
                var destPath = Path.Combine(pluginPath, (string)filename);
                Directory.Move(sourcePath, destPath);
            }
            loadPlugins();
        }

        private void btn_startLauncher_Click(object sender, RoutedEventArgs e)
        {
            if (chbx_admin.IsChecked.Value)
            {
                ProcessStartInfo info = new ProcessStartInfo(xivLauncherPath);
                info.UseShellExecute = true;
                info.Verb = "runas";
                Process.Start(info);
            }
            else
            {
                ProcessStartInfo info = new ProcessStartInfo(xivLauncherPath);
                Process.Start(info);
            }
        }

        private void getAllProfiles()
        {
            if (settings.Count == 0)
            {
                Console.WriteLine("AppSettings is empty.");
            }
            else
            {
                foreach (var key in settings.AllKeys)
                {
                    if (key.Contains("PROFILE"))
                    {
                        combo_profileList.Items.Add(key.Replace("PROFILE", ""));
                    }
                }
            }
        }

        private void saveList()
        {

            List<string> installedPluginsList = new List<string>();

            foreach (var path in Directory.GetDirectories(pluginPath))
            {
                var filename = System.IO.Path.GetFileName(path); // file name
                //installedPluginsList.Add(filename);
            }

            string value = String.Join(",", installedPluginsList.Select(i => i.ToString()).ToArray());

            var test = value.Split(',').Select(s => s.ToString()).ToArray();

            List<string> testList = new List<string>(test);

            
        }
        static void ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                Console.WriteLine(result);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
        }

        private void btn_saveProfile_Click(object sender, RoutedEventArgs e)
        {
            List<string> installedPluginsList = new List<string>();

            foreach (var path in Directory.GetDirectories(pluginPath))
            {
                var filename = System.IO.Path.GetFileName(path); // file name
                installedPluginsList.Add(filename);
            }

            string value = String.Join(",", installedPluginsList.Select(i => i.ToString()).ToArray());


            var profileName = "PROFILE" + tb_newProfile.Text;

            if (settings[profileName] == null)
            {
                settings.Add(profileName, value);
            }
            else
            {
                settings[profileName].Value = value;
            }

            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }

        private void btn_loadProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var profileName = "PROFILE" + combo_profileList.Text;
                string value;

                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[profileName] ?? "Not Found";
                value = result;

                var pluginListArray = value.Split(',').Select(s => s.ToString()).ToArray();

                List<string> pluginProfileList = new List<string>(pluginListArray);
                List<string> installedPlugins = new List<string>();

                foreach (var path in Directory.GetDirectories(pluginPath))
                {
                    var filename = Path.GetFileName(path);
                    installedPlugins.Add(filename);
                }

                //Install all Plugins from Profilelist
                foreach (string plugin in pluginProfileList)
                {
                    var destPath = Path.Combine(pluginPath, (string)plugin);
                    var sourcePath = Path.Combine(uninstalledPluginPath, (string)plugin);
                    if (!Directory.Exists(destPath))
                    {
                        Directory.Move(sourcePath, destPath);
                    }
                }

                //Remove all Plugins that are not in the List
                foreach(string plugin in installedPlugins)
                {
                    if (!pluginProfileList.Contains(plugin))
                    {
                        var sourcePath = Path.Combine(pluginPath, (string)plugin);
                        var destPath = Path.Combine(uninstalledPluginPath, (string)plugin);
                        Directory.Move(sourcePath, destPath);
                    }
                }
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
            loadPlugins();
        }

        private void btn_settings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }
    }
}

/*
 //string kek = "SomeSetting";
            //ReadSetting(kek);

            var key = "TestKey";
            var value = "TestValue";

            //settings.Remove(key); //delete shit

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
 
 */