using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Animation;
using Com.Airbnb.Lottie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOB_RadioApp.Api;
using System.Collections.ObjectModel;
using MOB_RadioApp.Models;
using MOB_RadioApp.Services;

namespace MOB_RadioApp.Droid
{
      
    [Activity(Label = "Radio", Theme = "@style/Theme.Splash",
        MainLauncher = true,
        NoHistory = true)]
    public class SplashActivity : Activity, Animator.IAnimatorListener
    {
        public void OnAnimationCancel(Animator animation)
        {
            
        }

        public void OnAnimationEnd(Animator animation)
        {
            
        }

        public void OnAnimationRepeat(Animator animation)
        {
            
        }

        public void OnAnimationStart(Animator animation)
        {
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_Splash);
            var animationView = FindViewById<LottieAnimationView>(Resource.Id.animation_view);
            animationView.AddAnimatorListener(this);
            //ApiService apiService = new ApiService();
            //AllStations.stations = apiService.GetStationsAsync("be").Result;
            // Create your application here
        }
    }
}