using System.Windows.Controls;

namespace My2Life___Modpack_Loader
{
    /// <summary>
    /// Interaktionslogik für ConnectionCheck.xaml
    /// </summary>
    public partial class ConnectionCheck : Page
    {
        public ConnectionCheck()
        {
            InitializeComponent();

            if(Utils.CheckInternetConnection())
            {
                Utils.ToggleNavbar(true);
                Utils.SetActive("Sync");
                Utils.NavigateToPage("Downloader.xaml");   
            } else
            {
                CheckText.Content = "Es besteht derzeitig keine Internetverbindung";
            }
        }
    }
}
