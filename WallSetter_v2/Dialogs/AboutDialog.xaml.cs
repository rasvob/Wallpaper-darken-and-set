using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WallSetter_v2.Dialogs
{
    /// <summary>
    /// Interaction logic for AboutDialog.xaml
    /// </summary>
    public partial class AboutDialog : UserControl
    {
        public AboutDialog()
        {
            InitializeComponent();
        }

        private void GithubButton_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(@"https://github.com/rasvob/Wallpaper-darken-and-set");
        }

        private void EmailButton_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(@"mailto://rasvob14@gmail.com");
        }
    }
}
