using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Wake_up_Robot_Client
{
    public class Alarm : INotifyPropertyChanged
    {
        #region variables

        private DateTime dateTime;
        private String description;
        private Boolean enabled;
        private TimeSpan recurrencyInterval;
        private DateTime endRecurrency;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        #endregion //variables

        #region properties
        /// <summary>
        /// Gets or sets the date and time of the alarm
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                return this.dateTime;
            }
            set
            {
                this.dateTime = value;
                NotifyPropertyChanged("DateTime");
            }
        }

        [XmlIgnoreAttribute]
        /// <summary>
        /// Returns a description in plaintext about the date and time of the alarm
        /// </summary>
        public String DateTimeDescription
        {
            get
            {
                String date;

                var timetillAlarm = dateTime.Subtract(DateTime.Today);
                switch (timetillAlarm.Days)
                {
                    case (0):
                        date= "vandaag";
                        break;
                    case(1):
                        date = "morgen";
                        break;
                    case (2):
                        date = "overmorgen";
                        break;
                    default:
                        date = dateTime.ToString("dddd") + " " + dateTime.Day + " " + dateTime.ToString("MMMM");
                        break;
                }

                return date + " om " + dateTime.TimeOfDay.ToString(@"hh\:mm");
            }
        }

        /// <summary>
        /// Gets or sets the description of the alarm
        /// </summary>
        public String Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                NotifyPropertyChanged("Description");
            }
        }

        [XmlAttribute("Enabled")]
        /// <summary>
        /// Gets or sets whether the alarm is enabled
        /// </summary>
        public Boolean Enabled
        {
            get
            {
                return this.enabled;
            }

            set
            {
                this.enabled = value;
                NotifyPropertyChanged("Enabled");
            }
        }

        [XmlIgnoreAttribute]
        /// <summary>
        /// Gets or sets the interval of the recurrency
        /// Returns null if there is no recurrency
        /// </summary>
        public TimeSpan RecurrencyInterval
        {
            get
            {
                return recurrencyInterval;
            }
            set
            {
                recurrencyInterval = value;
            }
        }

        [XmlElement("RecurrencyInterval")]
        /// <summary>
        /// Gets or sets the interval of the recurrency as a string so it can be serialized/desirialized
        /// Returns null if there is no recurrency
        /// </summary>
        public String RecurrencyIntervalAsString
        {
            get
            {
                if (recurrencyInterval != null)
                {
                    return recurrencyInterval.ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                recurrencyInterval = TimeSpan.Parse(value);
                NotifyPropertyChanged("RecurrencyInterval");
            }
        }
        
        /// <summary>
        /// Gets or sets the end of the recureny
        /// Returns null if there is no recurrency
        /// </summary>
        public DateTime EndRecurrency
        {
            get
            {
                return endRecurrency;
            }
            set
            {
                endRecurrency = value;
                NotifyPropertyChanged("EndRecurrency");
            }
        }

        [XmlIgnoreAttribute]
        /// <summary>
        /// Returns a description in plaintext about the time of the interval
        /// </summary>
        public String RecurrencyIntervalDescription
        {
            get
            {
                int years = (int)recurrencyInterval.TotalDays/356;
                int months = (int)((int)(recurrencyInterval.TotalDays % 356) / 30);
                int days = (int)((int)(recurrencyInterval.TotalDays % 356) % 30);
                int hours = recurrencyInterval.Hours;
                int minutes = recurrencyInterval.Minutes;

                string result = "";


                if (years + months + days + hours + minutes > 0)
                {
                    #region specials
                    if (years == 1 && months == 0 && days == 0 && hours == 0 && minutes == 0) //Jaarlijks
                    {
                        result = "Jaarlijks";
                    }
                    else if (years == 0 && months == 1 && days == 0 && hours == 0 && minutes == 0) //Maandelijks
                    {
                        result = "Maandelijks";
                    }
                    else if (years == 0 && months == 0 && days == 1 && hours == 0 && minutes == 0) //Dagelijks
                    {
                        result = "Dagelijks";
                    }
                    else if (years == 0 && months == 0 && days == 0 && hours == 1 && minutes == 0) //Elk uur
                    {
                        result = "ELk uur";
                    }
                    else if (years == 0 && months == 0 && days == 0 && hours == 0 && minutes == 1) //Elke minuut
                    {
                        result = "Elke minuut";
                    }
                    #endregion //specials
                    else
                    {
                        result += "Om de ";

                        if (years > 0)
                        {
                            result += years + " jaar,";
                        }

                        if (months > 0)
                        {
                            result += months;
                            if (months > 1)
                            {
                                result += " maanden,";
                            }
                            else
                            {
                                result += " maand,";
                            }
                        }
                        if (days > 0)
                        {
                            result += days;
                            if (days > 1)
                            {
                                result += " dagen,";
                            }
                            else
                            {
                                result += " dag,";
                            }
                        }

                        if (hours > 0)
                        {
                            result += hours + " uur,";
                        }

                        if (minutes > 0)
                        {
                            result += minutes;
                            if (days > 1)
                            {
                                result += " minuten,";
                            }
                            else
                            {
                                result += " minuut,";
                            }
                        }

                        result = result.Remove(result.Length-1); //remove last ","
                        //Last "," -> en
                        result = result.Substring(0, result.LastIndexOf(',')) + " en " + result.Substring(result.LastIndexOf(',') +1);
                    }
                }
                else
                {
                    result = "Geen";
                }
                return result;
                
            }
        }

        #endregion //properties

        #region public functions

        /// <summary>
        /// Initializes a new alarm object
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="description"></param>
        public Alarm()
        {
            this.enabled = false;
        }

        /// <summary>
        /// Initializes a new alarm object
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="description"></param>
        public Alarm(DateTime dateTime, String description)
        {
            this.dateTime = dateTime;
            this.description = description;
            this.enabled = false;
        }

        /// <summary>
        /// Initializes a new alarm object with a recurrency
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="description"></param>
        public Alarm(DateTime dateTime, String description, TimeSpan recurrencyInterval, DateTime endRecurrency)
        {
            this.dateTime = dateTime;
            this.description = description;
            this.recurrencyInterval = recurrencyInterval;
            this.endRecurrency = endRecurrency;
            this.enabled = false;
        }

        /// <summary>
        /// Disables the alarm
        /// </summary>
        public void Disable()
        {
            this.enabled = false;
        }

        /// <summary>
        /// Enables the alarm
        /// </summary>
        public void Enable()
        {
            this.enabled = true;
        }

        #endregion //public functions

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
