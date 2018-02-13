using System;
using System.Collections.Generic;
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
    /// Interaktionslogik für License_json.xaml
    /// </summary>
    public partial class License_json : Page
    {
        public License_json()
        {
            InitializeComponent();
            LicenseFrame.Content = "The MIT License (MIT)\r\n\r\nCopyright (c) 2007 James Newton-King\r\n\r\nPermission is hereby granted, free of charge, to any person obtaining a copy of\r\nthis software and associated documentation files (the \"Software\"), to deal in\r\nthe Software without restriction, including without limitation the rights to\r\nuse, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of\r\nthe Software, and to permit persons to whom the Software is furnished to do so,\r\nsubject to the following conditions:\r\n\r\nThe above copyright notice and this permission notice shall be included in all\r\ncopies or substantial portions of the Software.\r\n\r\nTHE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR\r\nIMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS\r\nFOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR\r\nCOPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER\r\nIN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN\r\nCONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.";
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Utils.NavigateToPage("About.xaml");
        }
    }
}
