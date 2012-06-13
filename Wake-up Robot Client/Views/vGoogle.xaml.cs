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
using System.Windows.Threading;

namespace Wake_up_Robot_Client.Views
{
    /// <summary>
    /// Interaction logic for vGoogle.xaml
    /// </summary>
    public partial class vGoogle : Window
    {
        cGoogle googleController;
        DispatcherTimer timer;
        string status, dots="";

        public vGoogle()
        {
            InitializeComponent();
            googleController = cGoogle.Instance;
            googleController.AuthDone += new AuthPogress(googleController_AuthDone);
            googleController.FetchDone += new FetchPogress(googleController_FetchDone);
            timer = new DispatcherTimer(DispatcherPriority.Background);
            timer.Interval = new TimeSpan(3000000); //300ms
            timer.Tick += new EventHandler(timer_Tick);
        }

        #region eventhandlers
        void timer_Tick(object sender, EventArgs e)
        {
            lblStatus.Content = status + dots;
            if (dots.Count() >= 3)
            {
                dots = "";
            }
            else
            {
                dots += ".";
            }
        }

        void googleController_FetchDone()
        {
            lstCalendars.ItemsSource = googleController.CalendarNames;
            lstCalendars.IsEnabled = true;
            timer.Stop();
            lblStatus.Content = "";
        }

        void googleController_AuthDone()
        {
            googleController.FetchCalendarNames();
            status = "Agendas ophalen";
        }
        #endregion

        private void bntLoadCalendars_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                status = "Authenticeren";
                timer.Start();
                googleController.Authenticate(txtAuthCode.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (lstCalendars.SelectedIndex > -1)
            {
                int result = googleController.GetAlarmsFromGcal(lstCalendars.SelectedIndex);
                if (result > 1)
                {
                    MessageBox.Show("Er werden " + result + " alarmen geïmporteerd");
                }
                else if (result > 0)
                {
                    MessageBox.Show("Er werd 1 alarm geïmporteerd");
                }
                else
                {
                    MessageBox.Show("Er werden geen alarmen geïmporteerd");
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void lstCalendars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCalendars.SelectedIndex > -1)
            {
                btnImport.IsEnabled = true;
            }
        }

        

        
    }
}
