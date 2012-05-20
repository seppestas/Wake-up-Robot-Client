using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wake_up_Robot_Client.Controllers;

namespace Wake_up_Robot_Client.Views
{
    /// <summary>
    /// Interaction logic for vImport.xaml
    /// </summary>
    public partial class vImport : Window
    {
        private cMain controller = cMain.Instance;
        public vImport()
        {
            InitializeComponent();
        }


        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)rdbGoogleAgenda.IsChecked)
            {
                controller.ShowImportGoogleWindow();
            }
            else
            {
                controller.ShowImportIcalWindow();
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void rdb_Checked(object sender, RoutedEventArgs e)
        {
            bool rdbChecked = false; //we assume there none of the radiobuttons is checked
            foreach (RadioButton rdb in radioButtons.Children)
            {
                if (!rdbChecked)
                {
                    if ((bool)rdb.IsChecked)
                    {
                        rdbChecked = true;
                    }
                }
            }
            btnNext.IsEnabled = rdbChecked; //button next is enabled if there is a checkbox checked
        }

        
    }
}
