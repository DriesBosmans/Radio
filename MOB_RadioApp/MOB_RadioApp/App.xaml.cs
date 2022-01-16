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
            
            Preferences.Set(Pref.selectedStation, null);
            InitializeComponent();
            Device.SetFlags(new[] { "Brush_Experimental" });

            MainPage = new MainPage();

        }

        protected override void OnStart()
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
