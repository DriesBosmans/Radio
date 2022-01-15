using Firebase.Database;
using MOB_RadioApp.Models;
using MOB_RadioApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MOB_RadioApp.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        FirebaseAuth FirebaseAuth = new FirebaseAuth();
        public AuthPopup()
        {
            InitializeComponent();     
        }

        private void BtnRegister_Clicked(object sender, EventArgs e)
        {
            _ = FirebaseAuth.Register(EnEmail.Text, EnPassword.Text);
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            await FirebaseAuth.LoginAsync(EnEmail.Text, EnPassword.Text);
            if (Preferences.Get(Pref.IsSignedIn, "") == Pref.True &&
                Preferences.Get(Pref.FirebaseRefreshToken, null) != null)
            {
                //MessagingCenter.Send(this, "loggedin");
                MessagingCenter.Send(this, "email", EnEmail.Text);
            }
        }
    }
}