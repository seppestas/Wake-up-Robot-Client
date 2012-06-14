using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Util;
using Google.Apis.Requests;
using Google;

namespace Wake_up_Robot_Client.Models
{
    class mGoogle
    {
        CalendarService service;       
        IList<CalendarListEntry> calendars;

        public mGoogle(CalendarService googleCalendarService)
        {
            service = googleCalendarService;
        }

        /// <summary>
        /// Returns the list of calendarnames in the calendarservice
        /// </summary>
        public List<String> CalendarNames
        {
            get
            {
                List<String> calendarSummaries = new List<string>();
                try
                {
                    calendars = service.CalendarList.List().Fetch().Items;
                }
                catch (GoogleApiRequestException ex)
                {
                    throw ex as Exception;
                }
                foreach (CalendarListEntry calendar in calendars)
                {
                    calendarSummaries.Add(calendar.Summary);
                }
                return calendarSummaries;
            }
        }

        /// <summary>
        /// Returns the list of future alarms in the google calendar
        /// </summary>
        /// <param name="index">Index of the google calendar in the calendar service</param>
        /// <returns></returns>
        public List<Alarm> GetAlarmsFromGcal(int index)
        {
            List<Alarm> alarms = new List<Alarm>();
            var calendar = calendars[index];
            var resource = service.Events.List(calendar.Id);
            resource.TimeMin = DateTime.Now.ToString("yyy-MM-ddTHH:mm:ss") + "+" + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).ToString("hhmm");
            var events = resource.Fetch().Items;
            if (events != null)
            {
                foreach (Event evt in events)
                {
                    if (evt.Reminders.Overrides != null)
                    {
                        foreach (EventReminder reminder in evt.Reminders.Overrides)
                        {
                            if (reminder.Method == "popup")
                            {
                                alarms.Add(new Alarm(DateTime.Parse(evt.Start.DateTime) - new TimeSpan(0, 0, Convert.ToInt16(reminder.Minutes.Value * 60)), evt.Summary));
                            }
                        }
                    }
                }
            }
            return alarms;
        }
    }
}
