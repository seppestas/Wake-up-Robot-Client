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

namespace Wake_up_Robot_Client.Models
{
    class mGoogle
    {

        private NativeApplicationClient provider;
        IAuthorizationState state;
        private string authCode;
        CalendarService service;
        IList<CalendarListEntry> calendars;

        public mGoogle()
        {
            // Register the authenticator. The Client ID and secret have to be copied from the API Access
            // tab on the Google APIs Console.
            provider = new NativeApplicationClient(GoogleAuthenticationServer.Description);
            provider.ClientIdentifier = "528532804655.apps.googleusercontent.com";
            provider.ClientSecret = "9QSFSAg0dE5CqC5F_Ot6MD8f";

            // Get the auth URL:
            state = new AuthorizationState(new[] { CalendarService.Scopes.Calendar.GetStringValue() });
            state.Callback = new Uri(NativeApplicationClient.OutOfBandCallbackUrl);
            Uri authUri = provider.RequestUserAuthorization(state);

            // Request authorization from the user (by opening a browser window):
            Process.Start(authUri.ToString());
            
        }

        public List<String> GetCalendars(string authCode)
        {
            List<String> calendarSummaries = new List<string>();
            this.authCode = authCode;
            var auth = new OAuth2Authenticator<NativeApplicationClient>(provider, GetAuthorization);
            service = new CalendarService(auth);


            calendars = service.CalendarList.List().Fetch().Items;
            foreach (CalendarListEntry calendar in calendars)
            {
                calendarSummaries.Add(calendar.Summary);
            }
            return calendarSummaries;
        }

        public List<Alarm> GetAlarmsFromGcal(int index)
        {
            List<Alarm> alarms = new List<Alarm>();
            var calendar = calendars[index];
            var resource = service.Events.List(calendar.Id);
            resource.TimeMin = DateTime.Now.ToString("yyy-MM-ddTHH:mm:ss") + "+" + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).ToString("hhmm");
            var events = resource.Fetch().Items;
            foreach (Event evt in events)
            {
                foreach (EventReminder reminder in evt.Reminders.Overrides)
                {
                    if (reminder.Method == "popup")
                    {
                        alarms.Add(new Alarm(DateTime.Parse(evt.Start.DateTime) - new TimeSpan(0, 0, Convert.ToInt16(reminder.Minutes.Value * 60)),evt.Summary));
                    }
                }
            }

            return alarms;
        }

        private IAuthorizationState GetAuthorization(NativeApplicationClient arg)
        {
            return arg.ProcessUserAuthorization(authCode, state);
        } 
    }
}
