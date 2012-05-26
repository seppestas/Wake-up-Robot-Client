using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDay.iCal;
using DDay.iCal.Components;
using DDay.iCal.DataTypes;
using DDay.iCal.Serialization;

namespace Wake_up_Robot_Client.Models
{
    /// <summary>
    ///     Model that reads events from a icalendar TextReader, and returs them as a list of alarms
    /// </summary>
    /// <remarks>
    ///     This model uses the DDay.iCal library written and Copyrighted by J. Douglas Day <doug@ddaysoftware.com>.
    /// </remarks>
    static class mICAL
    {

        /// <summary>
        /// Reads an ical file and returns the list of alarms contained in the ical.
        /// </summary>
        /// <param name="icalFileLocation">Ical file location</param>
        /// <returns>list of alarms contained in the ical</returns>
        static public List<Alarm> GetAlarms(String icalFileLocation)
        {
            iCalendar ical;
            List<Alarm> alarms = new List<Alarm>();

            ical = iCalendar.LoadFromFile(@icalFileLocation);
            foreach (Event evt in ical.Events)
            {
                foreach (DDay.iCal.Components.Alarm alarm in evt.Alarms)
                {
                    Date_Time dateTime = evt.Start + alarm.Trigger.Duration;
                    Alarm newAlarm = new Alarm(dateTime.Value, evt.Summary);
                    if (evt.RRule != null)
                    {
                        newAlarm.RecurrencyInterval = GetRecurrence(evt);
                        newAlarm.EndRecurrency = evt.RRule[0].Until.Value;
                    }
                    alarms.Add(newAlarm);
                }
            }

            return alarms;
        }

        static private TimeSpan GetRecurrence(Event evt)
        {
            TimeSpan RecurrencyInterval = new TimeSpan();
            if (evt.RRule != null)
            {
                foreach (Recur r in evt.RRule)
                {
                    switch (r.Frequency)
                    {
                        case Recur.FrequencyType.SECONDLY:
                            RecurrencyInterval.Add(new TimeSpan(0,0,0,r.Interval));
                            break;
                        case Recur.FrequencyType.MINUTELY:
                            RecurrencyInterval.Add(new TimeSpan(0,0,r.Interval,0));
                            break;
                        case Recur.FrequencyType.HOURLY:
                            RecurrencyInterval.Add(new TimeSpan(0,r.Interval,0,0));
                            break;
                        case Recur.FrequencyType.DAILY:
                            RecurrencyInterval.Add(new TimeSpan(r.Interval,0,0,0));
                            break;
                        case Recur.FrequencyType.WEEKLY:
                            RecurrencyInterval.Add(new TimeSpan(r.Interval*7,0,0,0));
                            break;
                        case Recur.FrequencyType.MONTHLY:
                            RecurrencyInterval.Add(new TimeSpan(r.Interval*30,0,0,0));
                            break;
                        case Recur.FrequencyType.YEARLY:
                            RecurrencyInterval.Add(new TimeSpan(r.Interval*356,0,0,0));
                            break;
                    }
                }
            }
            return RecurrencyInterval;
        }
    }
}