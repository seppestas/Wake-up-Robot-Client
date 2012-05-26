using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Util;
//using DotNetOpenAuth.OAuth2;

namespace Wake_up_Robot_Client.Models
{
    //class mGoogle
    //{
    //    private string apiKey = "AIzaSyDhY_4M7P5GGr0XTBqHMXTTTkfr3j_BgJI";

    //    public mGoogle(string username, string password)
    //    {
    //        // Register the authenticator.
    //        var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description);
    //        provider.ClientIdentifier = "528532804655.apps.googleusercontent.com";
    //        provider.ClientSecret = "9QSFSAg0dE5CqC5F_Ot6MD8f";
    //        var auth = new OAuth2Authenticator<NativeApplicationClient>(provider, GetAuthorization);

    //        // Create the service.
    //        var service = new CalendarService(auth);
    //        CalendarList results = service.CalendarList.List().Fetch();
    //        Console.WriteLine("Lists:");

    //    }

    //    private static IAuthorizationState GetAuthorization(NativeApplicationClient arg)
    //    {
    //        // Get the auth URL:
    //        IAuthorizationState state = new AuthorizationState(new[] { CalendarService.Scopes.Calendar.GetStringValue() });
    //        state.Callback = new Uri(NativeApplicationClient.OutOfBandCallbackUrl);
    //        Uri authUri = arg.RequestUserAuthorization(state);

    //        // Request authorization from the user (by opening a browser window):
    //        Process.Start(authUri.ToString());
    //        Console.Write("  Authorization Code: ");
    //        string authCode = Console.ReadLine();
    //        Console.WriteLine();

    //        // Retrieve the access token by using the authorization code:
    //        return arg.ProcessUserAuthorization(authCode, state);
    //    }
        
    //}
}
