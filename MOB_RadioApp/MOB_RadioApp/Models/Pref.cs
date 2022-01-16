using System;
using System.Collections.Generic;
using System.Text;

namespace MOB_RadioApp.Models
{
    public static class Pref
    {
        //Sets the number of stations returned by the api
        public static int Limit = 160;

        //Sets the current region the user is in
        public static string Region = "be";

        //Authentication for DarFm API
        public const uint Token = 3360242197;

        //Authentication for Firebase API
        public const string webApiKey = "AIzaSyBGmWlNQ22x1Tep-DyIu4G4xmWd0dk9j18";

        //These strings are Preferences properties
        public const string selectedGenre = "selectedGenre";
        public const string selectedLanguage = "selectedLanguage";
        public const string selectedStation = "selectedStation";
        public const string IsSignedIn = "IsSignedIn";
        public const string True = "True";
        public const string FirebaseToken = "FirebaseToken";
        public const string FirebaseRefreshToken = "FirebaseRefreshToken";
        public const string selectedCountry = "selectedCountry";
    }
}
