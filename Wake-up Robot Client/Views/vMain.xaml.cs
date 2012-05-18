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

namespace Wake_up_Robot_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class vMain : Window
    {
        private cMain controller;
        private cProgrammer prog;
        public vMain()
        {
            try
            {
                InitializeComponent();
                controller = cMain.Instance; //Get the singleton
                DataContext = controller;
                foreach (string s in SerialPort.GetPortNames())
                {
                    comboBox1.Items.Add(s);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            Alarm  b = new Alarm(new DateTime(2012, 5, 11, 11, DateTime.Now.Minute+1, 00), "test");
            prog.ProgramAlarm(b);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            controller.StoreAlarms();
        }

        private void portlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            prog = new cProgrammer(comboBox1.SelectedItem.ToString());
        }

    }
}
