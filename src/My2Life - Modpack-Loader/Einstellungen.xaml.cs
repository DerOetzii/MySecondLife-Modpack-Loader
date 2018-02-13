using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace My2Life___Modpack_Loader
{
    /// <summary>
    /// Interaktionslogik für Einstellungen.xaml
    /// </summary>
    public partial class Einstellungen : Page
    {
        public Einstellungen()
        {
            InitializeComponent();

            PathTextbox.Text = Utils.GetGTMPLocation();
            GTAVLocation.Text = Utils.GetGTAVLocation();
        }

        private void GTMPSearch_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            using (var Dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                Dialog.Description = "Wähle deinen GT-MP Ordner aus.";

                System.Windows.Forms.DialogResult Result = Dialog.ShowDialog();
               

                if(Result == System.Windows.Forms.DialogResult.OK)
                {
                    if(!File.Exists(Dialog.SelectedPath + @"\GrandTheftMultiplayer.Launcher.exe"))
                    {

                        MessageBox.Show("Bitte wähle einen gültigen GT-MP Ordner.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    } else
                    {
                        RegistryKey key;
                        key = Registry.CurrentUser.CreateSubKey(@"Software\My2Life");
                        key.SetValue("Directory", Dialog.SelectedPath);
                        MainWindow.GTMPDirectory = Dialog.SelectedPath;
                        key.Close();
                        MainWindow Window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                        Window.MainFrame.Refresh();
                    }
                }
            }
        }
    }
}
