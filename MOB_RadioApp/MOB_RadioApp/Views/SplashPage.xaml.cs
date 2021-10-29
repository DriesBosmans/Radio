using MOB_RadioApp.Models;
using MOB_RadioApp.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MOB_RadioApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage
    {
        private double _scale = 0;
        public SplashPage()
        {
            InitializeComponent();
            _scale = img.Scale;
            //Init();
            AnimationService.AnimateBackground(PurpleView,30000);
            Task.Run(AnimateWhite);
            Task.Run(AnimateImage);
            
        }
        private async void AnimateImage()
        {
            await img.ScaleTo(_scale * 1.5, 5000, easing: Easing.SinInOut);
        }

        private async void AnimateWhite()
        {
            uint time = 2000;
            Action<double> toWhite = input => Whitescreen.Opacity = input;
            await Task.Delay(3000);
             Whitescreen.Animate(name: "toWhite", callback: toWhite, start: 0, end: 1, length: time, easing: Easing.SinInOut);
           // await Task.Delay(2000);
            //Application.Current.MainPage = new MainPage();

        }


        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //await Shell.Current.GoToAsync("//LoginPage");
            Application.Current.MainPage = new MainPage();
        }
        
    }
}