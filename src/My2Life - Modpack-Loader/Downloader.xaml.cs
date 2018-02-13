using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace My2Life___Modpack_Loader
{
    /// <summary>
    /// Interaktionslogik für Downloader.xaml
    /// </summary>
    public partial class Downloader : Page
    {
        public Hashes FileHashes;

        public object Download_DownloadFileCompleted { get; private set; }
        public Stopwatch sw = new Stopwatch();

        public Downloader()
        {
            InitializeComponent();

            DownloadGrid.Margin = new Thickness(-600, 383, 0, 0);

            List<NeededFiles> items = new List<NeededFiles>();

            using (WebClient Client = new WebClient())
            {
                string Content = Client.DownloadString("https://mods.my2.life/hashes.json");
                FileHashes = JsonConvert.DeserializeObject<Hashes>(Content);
                items.Add(new NeededFiles() { Dateiname = "dlc.rpf", Hash = FileHashes.Dlc.Hash, Size = FileHashes.Dlc.Size });
                items.Add(new NeededFiles() { Dateiname = "update.rpf", Hash = FileHashes.Update.Hash, Size = FileHashes.Update.Size });
                items.Add(new NeededFiles() { Dateiname = "OpenIV.asi", Hash = FileHashes.OpenIV.Hash, Size = FileHashes.OpenIV.Size });


            }
            NeedFilesList.ItemsSource = items;

            //Items leeren
            items = new List<NeededFiles>();

            string UpdatePath = Utils.GetGTAVLocation() + @"\mods\update\update.rpf";
            string DlcPath = Utils.GetGTAVLocation() + @"\mods\update\x64\dlcpacks\MY2LIFE\dlc.rpf";
            string OpenIVPath = Utils.GetGTMPLocation() + @"\bin\asis\OpenIV.asi";

            MainWindow.NeedToDownload.Clear();
            if (File.Exists(UpdatePath) && File.Exists(DlcPath) && File.Exists(OpenIVPath))
            {
                if (!MainWindow.LocalHashesCalculated)
                {
                    MainWindow.UpdatePathHash = Utils.GetFileSHAHash(UpdatePath);
                    MainWindow.DlcPathHash = Utils.GetFileSHAHash(DlcPath);
                    MainWindow.OpenIVPathHash = Utils.GetFileSHAHash(OpenIVPath);
                    MainWindow.LocalHashesCalculated = true;
                }
            }
            else
            {
                if (File.Exists(UpdatePath) && !MainWindow.LocalHashesCalculated)
                {
                    MainWindow.UpdatePathHash = Utils.GetFileSHAHash(UpdatePath);
                }
                if (File.Exists(DlcPath) && !MainWindow.LocalHashesCalculated)
                {
                    MainWindow.DlcPathHash = Utils.GetFileSHAHash(DlcPath);
                }
                if (File.Exists(OpenIVPath) && !MainWindow.LocalHashesCalculated)
                {
                    MainWindow.OpenIVPathHash = Utils.GetFileSHAHash(OpenIVPath);
                }
            }

            if (File.Exists(DlcPath))
            {

                if (MainWindow.DlcPathHash == FileHashes.Dlc.Hash)
                {
                    FileInfo UpdateInfo = new FileInfo(DlcPath);
                    items.Add(new NeededFiles() { Dateiname = "dlc.rpf", Hash = " " + MainWindow.DlcPathHash, Size = ((UpdateInfo.Length / 1024f) / 1024f).ToString("n2") + " MB" });
                }
                else
                {
                    FileInfo UpdateInfo = new FileInfo(DlcPath);
                    items.Add(new NeededFiles() { Dateiname = "dlc.rpf", Hash = " " + MainWindow.DlcPathHash, Size = ((UpdateInfo.Length / 1024f) / 1024f).ToString("n2") + " MB" });
                    MainWindow.NeedToDownload.Add("dlc.rpf");
                }
            }
            else
            {
                MainWindow.NeedToDownload.Add("dlc.rpf");
                items.Add(new NeededFiles() { Dateiname = "dlc.rpf", Hash = " Datei nicht vorhanden", Size = "0 MB" });
            }

            if (File.Exists(UpdatePath))
            {

                if (MainWindow.UpdatePathHash == FileHashes.Update.Hash)
                {
                    FileInfo UpdateInfo = new FileInfo(UpdatePath);
                    items.Add(new NeededFiles() { Dateiname = "update.rpf", Hash = " " + MainWindow.UpdatePathHash, Size = ((UpdateInfo.Length / 1024f) / 1024f).ToString("n2") + " MB" });
                }
                else
                {
                    FileInfo UpdateInfo = new FileInfo(UpdatePath);
                    items.Add(new NeededFiles() { Dateiname = "update.rpf", Hash = " " + MainWindow.UpdatePathHash, Size = ((UpdateInfo.Length / 1024f) / 1024f).ToString("n2") + " MB" });
                    MainWindow.NeedToDownload.Add("update.rpf");
                }
            }
            else
            {
                MainWindow.NeedToDownload.Add("update.rpf");
                items.Add(new NeededFiles() { Dateiname = "update.rpf", Hash = " Datei nicht vorhanden", Size = "0 MB" });
            }

            if (File.Exists(OpenIVPath))
            {

                if (MainWindow.OpenIVPathHash == FileHashes.OpenIV.Hash)
                {
                    FileInfo UpdateInfo = new FileInfo(OpenIVPath);
                    items.Add(new NeededFiles() { Dateiname = "OpenIV.asi", Hash = " " + MainWindow.OpenIVPathHash, Size = ((UpdateInfo.Length / 1024f) / 1024f).ToString("n2") + " MB" });
                }
                else
                {
                    FileInfo UpdateInfo = new FileInfo(OpenIVPath);
                    items.Add(new NeededFiles() { Dateiname = "OpenIV.asi", Hash = " " + MainWindow.OpenIVPathHash, Size = ((UpdateInfo.Length / 1024f) / 1024f).ToString("n2") + " MB" });
                    MainWindow.NeedToDownload.Add("OpenIV.asi");
                }
            }
            else
            {
                MainWindow.NeedToDownload.Add("OpenIV.asi");
                items.Add(new NeededFiles() { Dateiname = "OpenIV.asi", Hash = " Datei nicht vorhanden", Size = "0 MB" });
            }

            LocalFilesList.ItemsSource = items;

            if (MainWindow.NeedToDownload.Count == 0)
            {
                NoUpdateRequired.Margin = new Thickness(464, 378, 0, 0);
                SyncStartButton.IsEnabled = false;
            }
            else
            {
                NoUpdateRequired.Margin = new Thickness(-600, 378, 0, 0);
                SyncStartButton.IsEnabled = true;
            }


        }

        public class NeededFiles
        {
            public string Dateiname { get; set; }

            public string Hash { get; set; }
            public string Size { get; set; }
        }

        public class Update
        {
            public string Hash { get; set; }
            public string Size { get; set; }
        }

        public class Dlc
        {
            public string Hash { get; set; }
            public string Size { get; set; }
        }
        public class OpenIV
        {
            public string Hash { get; set; }
            public string Size { get; set; }
        }

        public class Hashes
        {
            public Update Update { get; set; }
            public Dlc Dlc { get; set; }
            public OpenIV OpenIV { get; set; }
        }


        private void NeedFilesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Bald...
        }

        private void SyncStartButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SyncStartButton.IsEnabled = false;
            MainWindow.LocalHashesCalculated = false;
            BeginDownloadMissingFiles();
        }

        private void Download_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                TimeSpan ts = TimeSpan.FromMilliseconds((e.TotalBytesToReceive - e.BytesReceived) * sw.ElapsedMilliseconds / e.BytesReceived);
                string Left = "";
                string Speed = "";
                if(ts.Hours > 0)
                {
                    Left = $"Ungefähr {ts.Hours} Stunde(n), {ts.Minutes} Minuten, {ts.Seconds} Sekunde(n)";
                }
                if(ts.Minutes <= 0)
                {
                    Left = $"Ungefähr {ts.Seconds} Sekunde(n)";
                } else
                {
                    Left = $"Ungefähr {ts.Minutes} Minute(n), {ts.Seconds} Sekunde(n)";
                }

                if((e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds) >= 1000)
                {
                    Speed = ((e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds) / 1024).ToString("n2") + "MB/s";
                } else
                {
                    Speed = (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("n2") + " kb/s";

                }

                DownloadProgress.Value = e.ProgressPercentage;
                DownloadSpeedLabel.Content = " Geschwindigkeit: " + Speed;
                DownloadTime.Content = " Verbleibend: " + Left;
                DownloadBytes.Content = $" Fortschritt: {((e.BytesReceived / 1024f) / 1024f).ToString("n2")}MB / {((e.TotalBytesToReceive / 1024f) / 1024f).ToString("n2")}MB ({e.ProgressPercentage}%)";
            }));

        }

        private void BeginDownloadMissingFiles()
        {
            if (MainWindow.NeedToDownload.Count == 0)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    DownloadGrid.Margin = new Thickness(-600, 383, 0, 0);
                    NoUpdateRequired.Margin = new Thickness(464, 378, 0, 0);
                    MainWindow Window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    Window.MainFrame.Refresh();
                }));
                Utils.ToggleNavbar(true);
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    DownloadGrid.Margin = new Thickness(337, 383, 0, 0);
                    NoUpdateRequired.Margin = new Thickness(-600, 378, 0, 0);
                }));
                Utils.ToggleNavbar(false);
            }

            foreach (string File in MainWindow.NeedToDownload)
            {

                if (File == "dlc.rpf")
                {
                    Directory.CreateDirectory(Utils.GetGTAVLocation() + @"\mods\update\x64\dlcpacks\MY2LIFE\");
                    Thread thread = new Thread(() =>
                    {
                        WebClient client = new WebClient();
                        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Download_DownloadProgressChanged);
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(Download_Completed);
                        sw.Start();
                        client.DownloadFileAsync(new Uri("https://mods.my2.life/dlc.rpf"), Utils.GetGTAVLocation() + @"\mods\update\x64\dlcpacks\MY2LIFE\dlc.rpf");
                    });
                    thread.Start();

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        DownloadFileLabel.Content = " Datei wird heruntergeladen: dlc.rpf";
                    }));
                    MainWindow.NeedToDownload.Remove("dlc.rpf");
                    break;
                }
                if (File == "update.rpf")
                {
                    Directory.CreateDirectory(Utils.GetGTAVLocation() + @"\mods\update");
                    Thread thread = new Thread(() =>
                    {
                        WebClient client = new WebClient();
                        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Download_DownloadProgressChanged);
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(Download_Completed);
                        sw.Start();
                        client.DownloadFileAsync(new Uri("https://mods.my2.life/update.rpf"), Utils.GetGTAVLocation() + @"\mods\update\update.rpf");
                    });
                    thread.Start();
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        DownloadFileLabel.Content = " Datei wird heruntergeladen: update.rpf";
                    }));
                    MainWindow.NeedToDownload.Remove("update.rpf");
                    break;
                }
                if (File == "OpenIV.asi")
                {
                    Directory.CreateDirectory(Utils.GetGTMPLocation() + @"\bin\asis");
                    Thread thread = new Thread(() =>
                    {
                        WebClient client = new WebClient();
                        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Download_DownloadProgressChanged);
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(Download_Completed);
                        sw.Start();
                        client.DownloadFileAsync(new Uri("https://mods.my2.life/OpenIV.asi"), Utils.GetGTMPLocation() + @"\bin\asis\OpenIV.asi");
                    });
                    thread.Start();
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        DownloadFileLabel.Content = " Datei wird heruntergeladen: OpenIV.asi";
                    }));
                    MainWindow.NeedToDownload.Remove("OpenIV.asi");
                    sw.Reset();
                    break;
                }
            }
        }

        private void Download_Completed(object sender, AsyncCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                sw.Reset();
                BeginDownloadMissingFiles();
            }));
        }
    }
}
