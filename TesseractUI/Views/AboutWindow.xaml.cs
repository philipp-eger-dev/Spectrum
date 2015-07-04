using System.IO;
using System.Windows;

namespace TesseractUI
{
    public partial class AboutWindow : Window
    {
        #region Constructor
        public AboutWindow()
        {
            InitializeComponent();

            this.TxtLicense.Text = File.ReadAllText(@"G:\Development\Private\TesseractUI\TesseractUI\LicenseAgreement.rtf");
        }
        #endregion

        #region UI Event Handler
        private void Button_Close_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_ProjectSite_OnClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Button_ProjectSite.Content.ToString());
        }
        #endregion
    }
}
