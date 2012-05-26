using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Wake_up_Robot_Client.Models;
using Wake_up_Robot_Client.Views;

namespace Wake_up_Robot_Client.Controllers
{
    /// <summary>
    /// Singleton class containing operational code for wake up robot client
    /// </summary>
    class cMain : INotifyPropertyChanged
    {
        #region private variables
        private static volatile cMain instance;
        private static object syncRoot = new Object();
        public event EventHandler AlarmsChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        ObservableCollection<Alarm> alarms;
        #endregion // private varibles
        
        #region properties
        /// <summary>
        /// Returns the list of alarms in the model. Can't be used to add alarms, use the AddAlarm method
        /// </summary>
        public ObservableCollection<Alarm> Alarms
        {
            get
            {
                return alarms;
            }
        }

        /// <summary>
        /// Returns the ObservableCollection of future alarms in the model to be displayed by the vieuw.
        /// </summary>
        public List<Alarm> FutureAlarms
        {
            get
            {
                //Only show alarms in the future and alarm that recure in the future
                return alarms.OrderBy(a => a.DateTime).Where(a => a.DateTime >= DateTime.Now || (a.EndRecurrency != null && a.EndRecurrency > DateTime.Now)).ToList<Alarm>();
            }
        }


        /// <summary>
        /// Returns a desctription of the date and time of the next alarm
        /// </summary>
        public String NextAlarmDescription
        {
            get
            {
                if (FutureAlarms != null && FutureAlarms.Count > 0)
                {
                    return FutureAlarms.First().DateTimeDescription;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns a desctription of the date and time of the next alarm
        /// </summary>
        public String TimeTillNextAlarmDescr
        {
            get
            {
                if (alarms != null && FutureAlarms!= null && FutureAlarms.Count > 0)
                {
                    DateTime time = FutureAlarms.First().DateTime;
                    TimeSpan timeTillNextAlarm = new TimeSpan((time - DateTime.Now).Ticks);
                    string result = "Volgende alarm binnen: ";
                    if (timeTillNextAlarm.Hours > 0)
                        result += timeTillNextAlarm.Hours.ToString() + "u";
                    if (timeTillNextAlarm.Minutes > 0)
                        result += timeTillNextAlarm.Minutes.ToString() + "m";
                    if (timeTillNextAlarm.Seconds > 0)
                        result += timeTillNextAlarm.Seconds.ToString() + "s";

                    return   result;
                }
                else
                {
                    return null;
                }
            }
        }

        public string[] PortNames
        {
            get
            {
                return cProgrammer.GetSerialPortNames();
            }
        }
        #endregion //properties

        #region public

        /// <summary>
        /// Returns the instance of the singleton
        /// </summary>
        public static cMain Instance
        {
            get
            {
                if (instance == null) //Check if an instance has been made before
                {
                    lock (syncRoot) //Lock the ability to create instances, so this thread is the only thread that can excecute a constructor
                    {
                        if (instance == null) //Check if another thread initialized while we locked the object class
                        {
                            //Constructor stuff goes here
                            instance = new cMain();
                        }
                    }
                }
                return instance;
            }
        }

        #region open windows
        /// <summary>
        /// Shows a new intance of the new alarm window
        /// </summary>
        public void ShowNewAlarmWindow()
        {
            vNewAlarm newAlarm = new vNewAlarm();
            newAlarm.Show();
        }

        public void ShowNewAlarmWindow(Alarm alarm)
        {
            vNewAlarm newAlarmWindow = new vNewAlarm(alarm);
            newAlarmWindow.Show();
        }

        public void ShowImportWindow()
        {
            vImport importWindow = new vImport();
            importWindow.Show();

        }

        public void ShowImportIcalWindow()
        {
            vICal iCalWindow = new vICal();
            iCalWindow.Show();
        }

        public void ShowImportGoogleWindow()
        {
            vGoogle googleWindow = new vGoogle();
            googleWindow.Show();
        }

        #endregion open windows

        public void AddAlarm(Alarm alarm)
        {
            if (alarm.DateTime < alarms.Last().DateTime && alarm.DateTime > DateTime.Now)
            {
                NotifyPropertyChanged("NextAlarmDescription");
            }
            alarms.Add(alarm);            
        }

        public void AddAlarms(List<Alarm> alarms)
        {
            foreach (Alarm alarm in alarms)
            {
                AddAlarm(alarm);
            }
        }

        public void RemoveAlarm(Alarm alarm)
        {
            alarms.Remove(alarm);
        }

        public void StoreAlarms()
        {
            mXML.StoreAlarms(new List<Alarm>(alarms));
        }

        //iCal
        public void LoadIcalAlarms(String icalFileLocation)
        {
            AddAlarms(mICAL.GetAlarms(icalFileLocation));
        }

        //Goolge
        public List<String> GetCalendarList(string username, string password)
        {
            //Models.mGoogle googleModel = new mGoogle(username, password);
            return null;
        }

        #endregion //public

        #region private

        //Constructor is private because cMain is a singleton
        private cMain()
        {
            alarms = new ObservableCollection<Alarm>(mXML.GetAlarms());
            alarms.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(alarms_CollectionChanged);
        }

        private void alarms_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnAlarmsChanged(EventArgs.Empty); //Trigger the AlarmsChanged Event so the vieuw can be notified
            StoreAlarms();
        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion //private

        protected virtual void OnAlarmsChanged(EventArgs e)
        {
            if (AlarmsChanged != null)
            {
                AlarmsChanged(this, e);
            }
        }
    }
}
