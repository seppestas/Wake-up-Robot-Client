using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDay.iCal;
using DDay.iCal.Components;
using DDay.iCal.DataTypes;

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
            
            ical = iCalendar.LoadFromFile(icalFileLocation);
            foreach (Event evt in ical.Events)
            {
                foreach (DDay.iCal.Components.Alarm icalAlarm in evt.Alarms)
                {
                    Alarm alarm = new Alarm();
                }
            }
           
            return alarms;
        }
    }

   