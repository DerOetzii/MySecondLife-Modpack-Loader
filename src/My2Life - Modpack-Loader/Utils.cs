using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace My2Life___Modpack_Loader
{
    public class Utils
    {
        public static bool CheckInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("https://my2.life"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static void NavigateToPage(string Name)
        {
            MainWindow Window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            Window.MainFrame.Navigate(new Uri(Name, UriKind.Relative));
        }

        public static void ToggleNavbar(bool Toggle)
        {
            MainWindow Window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            if (Toggle)
            {
                Window.Navbar_Settings.Visibility = Visibility.Visible;
                Window.Navbar_Sync.Visibility = Visibility.Visible;
                Window.Navbar_About.Visibility = Visibility.Visible;
            }
            else
            {
                Window.Navbar_Settings.Visibility = Visibility.Hidden;
                Window.Navbar_Sync.Visibility = Visibility.Hidden;
                Window.Navbar_About.Visibility = Visibility.Hidden;
            }
        }

        public static void SetActive(string Item)
        {
            MainWindow Window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();


            switch (Item)
            {
                case "Sync":
                    {
                        Window.Navbar_Sync.Foreground = new SolidColorBrush(Colors.Gray);
                        Window.Navbar_Settings.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3A6D9C"));
                        Window.Navbar_About.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3A6D9C"));
                        break;
                    }
                case "Einstellungen":
                    {
                        Window.Navbar_Settings.Foreground = new SolidColorBrush(Colors.Gray);
                        Window.Navbar_Sync.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3A6D9C"));
                        Window.Navbar_About.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3A6D9C"));
                        break;
                    }
                case "Informationen":
                    {
                        Window.Navbar_About.Foreground = new SolidColorBrush(Colors.Gray);
                        Window.Navbar_Settings.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3A6D9C"));
                        Window.Navbar_Sync.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3A6D9C"));
                        break;
                    }
            }
        }

        public static string GetMD5()
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            System.IO.FileStream stream = new System.IO.FileStream(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            md5.ComputeHash(stream);

            stream.Close();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < md5.Hash.Length; i++)
                sb.Append(md5.Hash[i].ToString("x2"));

            return sb.ToString().ToUpperInvariant();
        }

        public static string GetFileSHAHash(string File)
        {
            try
            {
                using (FileStream fs = new FileStream(File, FileMode.Open))
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    using (SHA1Managed sha1 = new SHA1Managed())
                    {
                        byte[] hash = sha1.ComputeHash(bs);
                        StringBuilder formatted = new StringBuilder(2 * hash.Length);
                        foreach (byte b in hash)
                        {
                            formatted.AppendFormat("{0:X2}", b);
                        }
                        return formatted.ToString();
                    }
                }
            } catch(Exception ex) { return "Unbekannt"; }
        }
        public static string GetGTMPLocation() => MainWindow.GTMPDirectory;
        public static string GetGTAVLocation()
        {

            string location = "";

            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Rockstar Games\GTAV"))
                {
                    if (key != null)
                    {
                        Object steam = key.GetValue("InstallFolderSteam");
                        if (steam != null)
                        {
                            location = steam.ToString().Replace("GTAV", "");
                        }
                    }
                }
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Rockstar Games\Grand Theft Auto V"))
                {
                    if (key != null)
                    {
                        Object socialclub = key.GetValue("InstallFolder");
                        if (socialclub != null)
                        {
                            location = socialclub.ToString();
                        }
                    }
                }
            }
            catch (Exception ex) {}
            //Console.WriteLine(location);
            return location;
        }
    }
}
