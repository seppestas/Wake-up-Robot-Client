using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Wake_up_Robot_Client.Models;
using Wake_up_Robot_Client.Vieuws;

namespace Wake_up_Robot_Client.Controllers
{
    /// <summary>
    /// Singleton class containing operational code for wake up robot client
    /// </summary>
    class cMain : INotifyPropertyChanged
    {
        private static volatile cMain instance;
        private static object syncRoot = new Object();

        ObservableCollection<Alarm> alarms;

        public event PropertyChangedEventHandler PropertyChanged;
        
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
        public ObservableCollection<Alarm> FutureAlarms
        {
            get
            {
                //Only show alarms in the future and alarm that recure in the future
                var fAlarms = new ObservableCollection<Alarm>(alarms.Where(a => a.DateTime >= DateTime.Now || (a.EndRecurrency != null && a.EndRecurrency > DateTime.Now)));
                return fAlarms;
            }
        }

        /// <summary>
        /// Returns a desctription of the date and time of the next alarm
        /// </summary>
        public String NextAlarmDescription
        {
            get
            {
                if (alarms != null)
                {
                    return alarms
                        .Where(a => a.DateTime >= DateTime.Now)
                        .Last().DateTimeDescription;
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
                if (alarms != null)
                {
                    DateTime time = alarms
                        .Where(a => a.DateTime >= DateTime.Now)
                        .Last().DateTime;
                    time = new DateTime((time - DateTime.Now).Ticks);
                    return "Volgende alarm binnen: " + time.ToString("hhu mm'm' ss's'");
                    
                }
                else
                {
                    return null;
                }
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
                            instance = new cMain();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Shows a new intance of the new alarm window
        /// </summary>
        public void NewAlarmWindow()
        {
            vNewAlarm newAlarm = new vNewAlarm();
            newAlarm.Show();
        }

        public void NewAlarmWindow(Alarm alarm)
        {
            vNewAlarm newAlarm = new vNewAlarm(alarm);
            newAlarm.Show();
        }

        public void AddAlarm(Alarm alarm)
        {
            if (alarm.DateTime < alarms.Last().DateTime && alarm.DateTime > DateTime.Now)
            {
                NotifyPropertyChanged("NextAlarmDescription");
            }
            alarms.Add(alarm);            
        }

        public void RemoveAlarm(Alarm alarm)
        {
            alarms.Remove(alarm);
        }

        public void StoreAlarms()
        {
            mXML.StoreAlarms(new List<Alarm>(alarms));
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
            FutureAlarms.Clear(); //Needed to trigger the CollectionChanged event of FutureAlarms
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
    }
}
