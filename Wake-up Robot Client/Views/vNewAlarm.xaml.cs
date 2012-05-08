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

namespace Wake_up_Robot_Client.Vieuws
{
    /// <summary>
    /// Interaction logic for vNewAlarm.xaml
    /// </summary>
    public partial class vNewAlarm : Window
    {
        private cMain controller;

        public vNewAlarm()
        {
            Initialize();
        }

        public vNewAlarm(Alarm alarm)
        {
            Initialize();
            
            txtDescription.Text = alarm.Description;
            txtTime.Text = alarm.DateTime.ToString("HH:mm:ss");
            txtDate.Text = alarm.DateTime.ToString("dd/MM/yyy");
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = DateTime.Parse(txtDate.Text + " " + txtTime.Text);
            controller.AddAlarm(new Alarm(dateTime, txtDescription.Text));
            controller.StoreAlarms();
            this.Close();
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime date = (DateTime)calendar.SelectedDate;
            txtDate.Text = date.ToString("dd/MM/yyy");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            calendar.SelectedDate = DateTime.Today;
        }

        private void Initialize()
        {
            try
            {
                InitializeComponent();
                controller = cMain.Instance; //Get the singleton
                DataContext = controller;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
