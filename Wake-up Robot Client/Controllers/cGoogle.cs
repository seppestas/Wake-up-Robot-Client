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
using Wake_up_Robot_Client.Models;
using System.ComponentModel;

namespace Wake_up_Robot_Client.Controllers
{
    public delegate void AuthPogress();
    public delegate void FetchPogress();
    class cGoogle
    {
        private static object syncRoot = new Object();
        private static volatile cGoogle instance;
        private NativeApplicationClient provider;
        private IAuthorizationState state;
        private string authCode;
        private mGoogle googleModel;
        cMain controller = cMain.Instance;
        BackgroundWorker authWorker;
        BackgroundWorker fetchWorker;
        public event AuthPogress AuthDone;
        public event FetchPogress FetchDone;
        private List<String> calendarNames;

        /// <summary>
        /// Returns the instance of the google controller singleton
        /// </summary>
        public static cGoogle Instance
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
                            instance = new cGoogle();
                        }
                    }
                }
                return instance;
            }
        }

        public List<String> CalendarNames
        {
            get
            {
                return calendarNames;
            }
        }

        private cGoogle()
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
            // Initialize background workers
            authWorker = new BackgroundWorker();
            fetchWorker = new BackgroundWorker();
            authWorker.WorkerReportsProgress = true;
            authWorker.WorkerSupportsCancellation = true;
            authWorker.DoWork += new DoWorkEventHandler(authWorker_DoWork);
            authWorker.ProgressChanged += new ProgressChangedEventHandler(authWorker_ProgressChanged);
            fetchWorker.WorkerReportsProgress = true;
            fetchWorker.WorkerSupportsCancellation = true;
            fetchWorker.DoWork += new DoWorkEventHandler(fetchWorker_DoWork);
            fetchWorker.ProgressChanged += new ProgressChangedEventHandler(fetchWorker_ProgressChanged);
        }


        public void FetchCalendarNames()
        {
            if (!fetchWorker.IsBusy)
            {
                fetchWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Get the alarms from the selected calendar
        /// </summary>
        /// <param name="index">Index of the calendar</param>
        /// <returns>Amount of alarms imported</returns>
        public int GetAlarmsFromGcal(int index)
        {
            List<Alarm> googleAlarms = googleModel.GetAlarmsFromGcal(index);
            controller.AddAlarms(googleAlarms);
            return googleAlarms.Count;
        }

        public void Authenticate(string authCode)
        {
            this.authCode = authCode;
            if (!authWorker.IsBusy)
            {
                authWorker.RunWorkerAsync();
            }
        }

        public void Cancel()
        {
            if (authWorker.IsBusy)
            {
                authWorker.CancelAsync();
            }
            if (fetchWorker.IsBusy)
            {
                fetchWorker.CancelAsync();
            }
        }

        

        void fetchWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 100)
            {
                if (FetchDone != null)
                {
                    FetchDone(); //Report that the fetchworker is done
                }
            }
        }

        void fetchWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            calendarNames = googleModel.CalendarNames;
            worker.ReportProgress(100);
        }

        void authWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 100)
            {
                if (AuthDone != null)
                {
                    AuthDone(); //Report that the authworker is done
                }
            }
        }

        void authWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            var auth = new OAuth2Authenticator<NativeApplicationClient>(provider, GetAuthorization);
            worker.ReportProgress(50);
            googleModel = new mGoogle(new CalendarService(auth));
            worker.ReportProgress(100);
        }

        private IAuthorizationState GetAuthorization(NativeApplicationClient arg)
        {
            return arg.ProcessUserAuthorization(authCode, state);
        } 
    }
}
