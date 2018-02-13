using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaktionslogik für About.xaml
    /// </summary>
    public partial class About : Page
    {
        public About()
        {
            InitializeComponent();
            var Version = Assembly.GetExecutingAssembly().GetName().Version;
            VersionLabel.Content = VersionLabel.Content +  $"{Version.Major}.{Version.Minor}.{Version.Build}.{Version.Revision}";
        }

        private void FontAwesomeLicense_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            Utils.NavigateToPage("License_Fontawesome.xaml");
        }

        private void NewtonsoftLicense_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Utils.NavigateToPage("License_json.xaml");
        }
    }
}
