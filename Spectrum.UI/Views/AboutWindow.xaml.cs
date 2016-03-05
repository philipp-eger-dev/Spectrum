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

            string licenseAgreementPath = Path.GetFullPath(Spectrum.UI.Properties.Settings.Default.LicenseAgreementPath);

            this.TxtLicense.Text = File.ReadAllText(licenseAgreementPath);
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
