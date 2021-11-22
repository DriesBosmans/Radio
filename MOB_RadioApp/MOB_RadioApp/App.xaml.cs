using MOB_RadioApp.Api;
using MOB_RadioApp.Models;
using MOB_RadioApp.Views;
using System;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace MOB_RadioApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Device.SetFlags(new[] { "Brush_Experimental" });
            Preferences.Set(ProjectSettings.selectedStation, null);
            Preferences.Set(ProjectSettings.selectedLanguage, null);
            MainPage = new MainPage();

        }

        protected override async void OnStart()
        {
     
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
