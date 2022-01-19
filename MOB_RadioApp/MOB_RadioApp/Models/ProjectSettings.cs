namespace MOB_RadioApp.Models
{
    public static class ProjectSettings
    {
        // Haven't found a secure way yet to store credentials
        
        //Authentication for DarFm API
        public const uint Token = 3360242197;

        //Authentication for Firebase API
        public const string webApiKey = "AIzaSyBGmWlNQ22x1Tep-DyIu4G4xmWd0dk9j18";

        //These strings are Preferences properties
        public const string selectedGenre = "selectedGenre";
        public const string selectedFilterLanguage = "selectedLanguage";
        public const string selectedStation = "selectedStation";
        public const string IsSignedIn = "IsSignedIn";
        public const string True = "True";
        public const string FirebaseToken = "FirebaseToken";
        public const string FirebaseRefreshToken = "FirebaseRefreshToken";
        public const string selectedCountry = "selectedCountry";
        public const string background = "background";
        public const string Email = "Email";
        public const string Language = "Language";
    }
}
