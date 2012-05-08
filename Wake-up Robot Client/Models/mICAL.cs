using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDay.iCal;

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
        /// Reads a Textreader and returns the list of alarms contained in the TextReader.
        /// </summary>
        /// <param name="icalReader"></param>
        /// <returns></returns>
        static public List<Alarm> GetAlarms(String icalFileLocation)
        {
            List<Alarm> alarms = new List<Alarm>();
            iCalendarCollection calendars = new iCalendarCollection();
            calendars.AddRange(iCalendar.LoadFromFile(icalFileLocation));
            foreach (iCalendar calendar in calendars)
            {
                foreach (Event calEvent in calendar.Events)
                {
                    foreach (IAlarm evAlarm in calEvent.Alarms)
                    {
                        Alarm alarm = new Alarm();
                        alarm.Description=calEvent.Description;

                        if (evAlarm.Trigger == (ITrigger)calEvent.DTEnd) //trigger of alarm on end of event
                        {
                            alarm.DateTime=calEvent.DTEnd.Value;
                        }
                        else
                        {
                            alarm.DateTime=calEvent.DTStart.Value;
                        }

                        calEvent.RecurrenceRules
                        
                    }
                }
            }
            return alarms;
        }
    }

   