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
        private cMain controller;
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
        }

        
    }
}
