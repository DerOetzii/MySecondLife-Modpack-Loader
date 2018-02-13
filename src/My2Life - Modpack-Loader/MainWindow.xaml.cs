using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace My2Life___Modpack_Loader
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    /// 

    public class Version
    {
        public string version { get; set; }
        public string changelog { get; set; }
    }
    public class ModLauncherHashes
    {
        public IList<string> hashes { get; set; }
    }

    public partial class MainWindow : Window
    {
        public static String GTMPDirectory = null;
        public static bool LocalHashesCalculated = false;
        public static string UpdatePathHash;
        public static string DlcPathHash;
        public static string OpenIVPathHash;
        public static List<string> NeedToDownload = new List<string>();
        public static List<string> ValidLauncherHashes = new List<string>();

        public bool Update = false;

        public MainWindow()
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;

            if (File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\update.bat"))
            {
                File.Delete(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\update.bat");
            }

            try
            {
                if (!File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Updater.exe"))
                {
                    using (WebClient Client = new WebClient())
                    {
                        Client.DownloadFileAsync(new Uri("https://mods.my2.life/Updater.exe"), Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Updater.exe");
                    }
                }
            } catch(Exception ex) {}


            try
            {
                using (WebClient Client = new WebClient())
                {
                    string Content = Client.DownloadString("https://mods.my2.life/modlauncher.hashes.json");
                    var Version = JsonConvert.DeserializeObject<ModLauncherHashes>(Content);

                    foreach(string tmp in Version.hashes)
                    {
                        ValidLauncherHashes.Add(tmp);
                    }
                }
                string Hash = Utils.GetMD5();
                if (!ValidLauncherHashes.Contains(Hash))
                {
                    MessageBox.Show($"Diese Datei wurde mutmaßlich manipuliert.\nBitte lade den Launcher erneut herunter, vielen Dank!\n\nWeitere Informationen:\n------------------------------\nAusgeführte Datei:\n{System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName}\n\nHash:\n{Hash}", "Kritischer Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex) { }

            try
            {
                using (WebClient Client = new WebClient())
                {
                    string Content = Client.DownloadString("https://mods.my2.life/version.json");
                    var Version = JsonConvert.DeserializeObject<Version>(Content);

                    var AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
                    string FormattedVersion = $"{AssemblyVersion.Major}.{AssemblyVersion.Minor}.{AssemblyVersion.Build}.{AssemblyVersion.Revision}";

                    if (Version.version != FormattedVersion)
                    {
                        MessageBoxResult DoUpdate = MessageBox.Show($"Ein Update mit der Version {Version.version} ist verfügbar.\n\nChangelog:\n{Version.changelog}\n\nMöchtest du das Update herunterladen?", "Update", MessageBoxButton.YesNo, MessageBoxImage.Information);
                        if (DoUpdate == MessageBoxResult.Yes)
                        {
                            Update = true;
                            using (var BW = new StreamWriter(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\update.bat", true))
                            {
                                BW.WriteLine($"TASKKILL /PID {Process.GetCurrentProcess().Id}");
                                BW.WriteLine($"start /d \"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\" Updater.exe");
                                BW.Close();
                            }
                            Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\update.bat");
                        }
                    }
                }
            } catch(Exception ex) { }

            Process[] ProcessName = Process.GetProcessesByName("Grand Theft Auto V");

            if (ProcessName.Length != 0)
            {
                MessageBox.Show("Beende zuerst GTA V!", "Achtung", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

            if (Registry.CurrentUser.OpenSubKey(@"Software\My2Life") == null)
            {
                RegistryKey GTMPRegistry = Registry.CurrentUser.OpenSubKey(@"Software\Classes\gtmp\DefaultIcon");
                if (GTMPRegistry != null && GTMPRegistry.GetValue(null).ToString() != null)
                {
                    GTMPDirectory = Path.GetDirectoryName(GTMPRegistry.GetValue(null).ToString());
                    if (Registry.CurrentUser.OpenSubKey(@"Software\My2Life") == null)
                    {
                        RegistryKey key;
                        key = Registry.CurrentUser.CreateSubKey(@"Software\My2Life");
                        key.SetValue("Directory", GTMPDirectory);
                        key.Close();
                    }
                }
            } else
            {
                RegistryKey key;
                key = Registry.CurrentUser.OpenSubKey(@"Software\My2Life");
                GTMPDirectory = key.GetValue("Directory").ToString();
                key.Close();
            }

            if(Update)
            {
                Application.Current.Shutdown();
            } else
            {
                Utils.ToggleNavbar(false);
                Utils.NavigateToPage("ConnectionCheck.xaml");
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                    DragMove();
            }
            catch (Exception ex) { }

        }

        private void Exit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Navbar_Sync_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Utils.SetActive("Sync");
            Utils.NavigateToPage("Downloader.xaml");
        }

        private void Navbar_Settings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Utils.SetActive("Einstellungen");
            Utils.NavigateToPage("Einstellungen.xaml");
        }

        private void Navbar_About_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Utils.SetActive("Informationen");
            Utils.NavigateToPage("About.xaml");
        }
    }
}
