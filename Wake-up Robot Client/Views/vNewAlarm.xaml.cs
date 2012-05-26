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
    /// Interaction logic for vNewAlarm.xaml
    /// </summary>
    public partial class vNewAlarm : Window
    {
        private cMain controller;
        Alarm editAlarm;
        private String[] recTypes ={"Dagelijks", "Weekelijks", "Maandelijks", "Jaarlijks"},
            typeValues={"dagen", "weken", "maanden", "jaren"};


        public vNewAlarm()
        {
            Initialize();
            btnAddEdit.Content = "Toevoegen";
            this.Title = "Alarm toevoegen";
            datePicker.SelectedDate = DateTime.Today;
            txtTime.Text = DateTime.Now.ToString("HH:mm");
            recDatePicker.SelectedDate = DateTime.Today;
            txtDescription.Text = "Nieuw alarm";
            txtDescription.Focus();
            txtDescription.SelectAll();
        }

        public vNewAlarm(Alarm alarm)
        {
            Initialize();
            btnAddEdit.Content = "Aanpassen";
            this.Title = "Alarm aanpassen";
            if (alarm != null)
            {
                editAlarm = alarm;
                txtDescription.Text = alarm.Description;
                txtTime.Text = alarm.DateTime.ToString("HH:mm");
                if (alarm.DateTime.Second > 0)
                {
                    txtTime.Text += alarm.DateTime.ToString("ss");
                }
                datePicker.SelectedDate = alarm.DateTime;
            }
            else
            {
                datePicker.SelectedDate = DateTime.Today;
                txtTime.Text = DateTime.Now.ToString("HH:mm:ss");
            }
            recDatePicker.SelectedDate = DateTime.Today;
        }

        private void btnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = (DateTime)datePicker.SelectedDate;
            dateTime = dateTime.Add(TimeSpan.Parse(txtTime.Text));
            if (editAlarm == null)
            {
                controller.AddAlarm(new Alarm(dateTime, txtDescription.Text));
            }
            else
            {
                editAlarm.Disable();
                editAlarm.DateTime = dateTime;
                editAlarm.Description = txtDescription.Text;
            }
            controller.StoreAlarms();
            this.Close();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            foreach (string type in recTypes)
            {
                cmbRecType.Items.Add(type);
            }
            cmbRecType.SelectedIndex = 0;
            fillCmbAmount();

        }

        private void chkRecure_Checked(object sender, RoutedEventArgs e)
        {
            stackRec.IsEnabled = (bool)chkRecure.IsChecked;
        }

        private void chkRecure_Unchecked(object sender, RoutedEventArgs e)
        {
            stackRec.IsEnabled = (bool)chkRecure.IsChecked;
        }

        private void cmbRecType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRecType.SelectedIndex >= 0)
            {
                lblValue.Content = typeValues[cmbRecType.SelectedIndex];
            }
            
        }

        private void fillCmbAmount()
        {
            for (int i = 1; i <= 30; i++)
            {
                cmbAmount.Items.Add(i);
            }
            cmbAmount.SelectedIndex = 0;
        }

        
    }
}
