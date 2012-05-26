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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wake_up_Robot_Client.Controllers;
using Wake_up_Robot_Client.Views;
using System.IO.Ports;
using System.Windows.Threading;

namespace Wake_up_Robot_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class vMain : Window
    {
        private cMain controller;
        private cProgrammer prog;
        private DispatcherTimer timer;
        public vMain()
        {
            try
            {
                InitializeComponent();
                controller = cMain.Instance; //Get the singleton
                DataContext = controller;
                controller.AlarmsChanged += new EventHandler(controller_AlarmsChanged);
                timer = new DispatcherTimer(DispatcherPriority.Background);
                timer.Start();
                timer.Tick += new EventHandler(timer_Tick);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            lblStatus.Content = controller.TimeTillNextAlarmDescr;
        }

        void controller_AlarmsChanged(object sender, EventArgs e)
        {
            lstAlarms.ItemsSource = controller.FutureAlarms; //update the list of future alarms
        }

        void prog_WorkerProgress(int progress, string state)
        {
            if (progress >= 100)
            {
                progressBar.Value = 0;
                lblStatus.Content = controller.TimeTillNextAlarmDescr;
            }
            else
            {
                progressBar.Value = progress;
                lblStatus.Content = state;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            controller.ShowNewAlarmWindow();
        }
        
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lstAlarms.SelectedItem != null)
            {
                controller.ShowNewAlarmWindow(lstAlarms.SelectedItem as Alarm);
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lstAlarms.SelectedItem != null)
            {
                controller.RemoveAlarm(lstAlarms.SelectedItem as Alarm);
            }
            
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            controller.ShowImportWindow();
        }

        private void btnProgram_Click(object sender, RoutedEventArgs e)
        {
            TestPort();
            Alarm  b = new Alarm(new DateTime(2012, 5, 11, 11, DateTime.Now.Minute+1, 00), "test");
            if (prog != null)
            {
                prog.ProgramAlarm(lstAlarms.SelectedItem as Alarm);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            controller.StoreAlarms();
        }

        private void portlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            prog = new cProgrammer(cmbPorts.SelectedItem.ToString());
            prog.WorkerProgress += new WriteProgress(prog_WorkerProgress);
            TestPort();
        }

        private void lstAlarms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = btnRemove.IsEnabled = lstAlarms.SelectedItem != null;
            TestPort();
        }

        private void TestPort()
        {
            if (prog != null && prog.Exists && lstAlarms.SelectedItem != null)
            {
                btnProgram.IsEnabled = true;
            }
            else
            {
                btnProgram.IsEnabled = false;
            }
        }
    }
}
