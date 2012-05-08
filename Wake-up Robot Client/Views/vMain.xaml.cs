﻿using System;
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
using Wake_up_Robot_Client.Vieuws;

namespace Wake_up_Robot_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class vMain : Window
    {
        private cMain controller;

        public vMain()
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            controller.NewAlarmWindow();
        }
        
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            controller.NewAlarmWindow(lstAlarms.SelectedItem as Alarm);
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {

            controller.RemoveAlarm(lstAlarms.SelectedItem as Alarm);
            
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnProgram_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            controller.StoreAlarms();
        }

    }
}