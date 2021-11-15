using System;
using System.Collections.Generic;
using System.Text;

namespace MOB_RadioApp.Models
{
    public static class ProjectSettings
    {
        //Sets the number of stations returned by the api
        public static int Limit = 160;

        //Sets the current region the user is in
        public static string Region = "be";

        //Authentication for DarFm API
        public const uint Token = 3360242197;

        //These strings are Preferences properties
        public static string selectedGenre = "selectedGenre";
        public static string selectedLanguage = "selectedLanguage";
        public static string selectedStation = "selectedStation";
    }
}
