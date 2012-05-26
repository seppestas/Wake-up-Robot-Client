using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Wake_up_Robot_Client.Controllers;

namespace Wake_up_Robot_Client.Views
{
    /// <summary>
    /// Interaction logic for vICal.xaml
    /// </summary>
    public partial class vICal : Window
    {
        cMain controller = cMain.Instance;
        public vICal()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".ics, .ical";
            ofd.Filter = "ICal bestanden |*.ics; *.ical";
            bool? result = ofd.ShowDialog();

            if ((bool)result)
            {
                txtFileLocation.Text = ofd.FileName;
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            string fileLocation = txtFileLocation.Text;
            try
            {
                if (File.Exists(fileLocation))
                {
                    controller.LoadIcalAlarms(txtFileLocation.Text);
                }
                else
                {
                    throw new FileNotFoundException("Het bestand \"" + fileLocation + "\" kon niet worden gevonden.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtFileLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnImport.IsEnabled = txtFileLocation.Text != "";
        }

        

        
    }
}
