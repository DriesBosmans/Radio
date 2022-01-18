using Firebase.Auth;
using Firebase.Database;
using MOB_RadioApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Firebase.Database.Query;
using System.Collections.ObjectModel;

namespace MOB_RadioApp.Services
{
    /// <summary>
    /// Only Register and login are currently used
    /// </summary>
    public class FirebaseAuth
    {
        //FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(ProjectSettings.webApiKey));
        private List<string> listFavs;
        public async Task Register(string email, string password)
        {
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(ProjectSettings.webApiKey));
            try
            {

                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
                string gettoken = auth.FirebaseToken;
                await App.Current.MainPage.DisplayAlert("Alert", "Welkom", "Ok");

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Foutje", ex.Message, "Oke");
            }
        }

        public async Task LoginAsync(string email, string password)
        {
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(ProjectSettings.webApiKey));
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
                var content = await auth.GetFreshAuthAsync();

                var serializedcontent = JsonConvert.SerializeObject(content);
                Preferences.Set(ProjectSettings.FirebaseRefreshToken, serializedcontent);
                Preferences.Set(ProjectSettings.IsSignedIn, ProjectSettings.True);
                Preferences.Set(ProjectSettings.Email, email);
                //listFavs = await GetFavourites();
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Foutje", "Ongeldige email of wachtwoord", "Oke");
            }
        }

        public async Task GetRefreshToken()
        {
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(ProjectSettings.webApiKey));
            try
            {
                var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get(ProjectSettings.FirebaseToken, ""));
                var refreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                Preferences.Set(ProjectSettings.FirebaseToken, JsonConvert.SerializeObject(refreshedContent));
                await App.Current.MainPage.DisplayAlert("success", "", "");
            }
            catch (Exception ex)
            {
                Preferences.Set(ProjectSettings.FirebaseToken, null);
                Preferences.Set(ProjectSettings.IsSignedIn, null);
                await App.Current.MainPage.DisplayAlert("fatal error", "", "");
            }
        }
        public async Task<List<string>> GetFavourites()
        {
           
            string json ="";
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(ProjectSettings.webApiKey));
            try
            {
                var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get(ProjectSettings.FirebaseRefreshToken, ""));
                var refreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                Preferences.Set(ProjectSettings.FirebaseRefreshToken, JsonConvert.SerializeObject(refreshedContent));
                json = savedfirebaseauth.User.PhotoUrl;
                listFavs = JsonConvert.DeserializeObject<List<string>>(json);
            }
            catch (Exception ex)
            {
                Preferences.Set(ProjectSettings.FirebaseToken, null);
                Preferences.Set(ProjectSettings.IsSignedIn, null);
                await App.Current.MainPage.DisplayAlert("fatal error", ex.Message, "ok");
            }
            if(json != null)
                MessagingCenter.Send(this, "list", json);
            return listFavs;
        }
        public async Task SetFavourite(string id, List<string> _favids)
        {
            string json;
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(ProjectSettings.webApiKey));
            try
            {
                var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get(ProjectSettings.FirebaseRefreshToken, ""));
                var refreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                Preferences.Set(ProjectSettings.FirebaseRefreshToken, JsonConvert.SerializeObject(refreshedContent));
                if (_favids == null)
                    _favids = new List<string>();
                _favids.Add(id);
                json = JsonConvert.SerializeObject(_favids);
                
                // Can't get this to work
                await authProvider.UpdateProfileAsync(Preferences.Get(ProjectSettings.FirebaseRefreshToken,""), "", json);
            }
            catch (Exception ex)
            {
                Preferences.Set(ProjectSettings.FirebaseToken, null);
                Preferences.Set(ProjectSettings.IsSignedIn, null);
                await App.Current.MainPage.DisplayAlert("fatal error", ex.Message, "ok");
            }
        }
        public async Task RemoveFavourite(string id, List<string> _favids)
        {
            string json;
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(ProjectSettings.webApiKey));
            try
            {
                var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get(ProjectSettings.FirebaseToken, ""));
                _favids.Remove(id);
                json = JsonConvert.SerializeObject(_favids);

                await authProvider.UpdateProfileAsync(ProjectSettings.FirebaseToken, "", json);
            }
            catch (Exception ex)
            {
                Preferences.Set(ProjectSettings.FirebaseToken, null);
                Preferences.Set(ProjectSettings.IsSignedIn, null);
                await App.Current.MainPage.DisplayAlert("fatal error", ex.Message, "oke");
            }
        }

    }
}
