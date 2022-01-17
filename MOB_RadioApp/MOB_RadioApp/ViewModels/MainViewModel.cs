using MediaManager;
using MOB_RadioApp.Api;
using MOB_RadioApp.css;
using MOB_RadioApp.Models;
using MOB_RadioApp.Popups;
using MOB_RadioApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinCountryPicker.Models;
using XamarinCountryPicker.Popups;
using XamarinCountryPicker.Utils;
using static MOB_RadioApp.Models.MetaInfo;

namespace MOB_RadioApp.ViewModels
{
    /// <summary>
    /// This is the main file in this project.
    /// 
    /// Because of how the tabview works, all the pages (stations, favourites, mediaplayer and settings) are
    /// on the same view. I've split them up into different controls, to keep things tidy.
    /// This file contains different sections (constructor, private fields, public properties, commands and
    /// methods. Methods are further split up according to their respective controls.
    /// 
    /// I've used different popups - based on the countrypicker popup - which use the Messagingcenter to pass info back into the mainviewmodel,
    /// because i couldn't get it to work with commands.
    /// 
    /// I've tried lot's of different things for selecting countries, pickers and such, nothing worked.
    /// I fell back on using the countrypicker, which worked nicely.
    /// Besides cleaning up, it was the last thing I did in this project
    /// 
    /// The countrypicker popup 
    /// </summary>
    public class MainViewModel : BaseViewModel2
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel()
        {
            InitializeAsync();

            // This lets this viewmodel know which filters have been applied and when
            MessagingCenter.Subscribe<FilterPopup>(this, "SettingsApplied", (sender) =>
            {
                _filteredStations = Filter(_unfilteredStations, Preferences.Get(Pref.selectedGenre, ""), Preferences.Get(Pref.selectedLanguage, ""));
                FilteredStations = _filteredStations;
                OnPropertyChanged(nameof(FilteredStations));
            });
            // This lets this viewmodel know who has logged in and when
            MessagingCenter.Subscribe<AuthPopup, string>(this, "email", (sender, arg) =>
            {
                Email = arg;
                OnPropertyChanged(nameof(Email)); IsSignedIn = true;
                Preferences.Set(Pref.IsSignedIn, Pref.True);
                OnPropertyChanged(nameof(IsSignedIn));
                _favourites = ConvertToCollection(SqlLiteService.GetFavourites().Result);
            
                OnPropertyChanged(nameof(FavouriteStations));

            });
        }

        #region Fields
        DarFmApiCall _darfmapi = new DarFmApiCall();
        public string SelectedGenre = Preferences.Get(Pref.selectedGenre, "");
        public string SelectedLanguage = Preferences.Get(Pref.selectedLanguage, "");
        private FilterChoices _filterChoices;
        private RangedObservableCollection<Station> _filteredbygenre = new RangedObservableCollection<Station>();
        private RangedObservableCollection<Station> _filteredbylanguage = new RangedObservableCollection<Station>();
        private RangedObservableCollection<Station> _filteredStations;
        private RangedObservableCollection<Station> _unfilteredStations;
        private RangedObservableCollection<Station> _favourites = new RangedObservableCollection<Station>();
        private Station _activeStation;
        private string _activeImg;
        private bool _isPlaying = false;
        private string _activeSong;
        private string _activeArtist;
        private string _searchText;
        private bool _isSignedIn;
        private bool isRefreshing;
        private string _email;
        private CountryModel _selectedCountry;
        private static Background _selectedBackground;
        #endregion

        #region Properties

        public string SearchText
        {
            get { return _searchText; }
            set { SetValue(ref _searchText, value); }
        }

        public FilterChoices ChoicesSelected
        {
            get { return _filterChoices; }
            set { SetValue(ref _filterChoices, value); }
        }

        public RangedObservableCollection<Station> FilteredStations
        {
            get { return _filteredStations; }
            set { SetValue(ref _filteredStations, value); }
        }
        public RangedObservableCollection<Station> FavouriteStations
        {
            get { return _favourites; }
            set { SetValue(ref _favourites, value); }
        }

        public bool IsRefreshing
        {
            get => isRefreshing;
            set { SetValue(ref isRefreshing, value); }
        }

        public Station ActiveStation
        {
            get { return _activeStation; }
            set
            {
                SetValue(ref _activeStation, value);
                OnPropertyChanged(nameof(ActiveImg));
            }
        }

        public string ActiveImg
        {
            get { return ActiveStation?.Imageurl; }
            set { SetValue(ref _activeImg, value); }
        }

        public string ActiveArtist
        {
            get { return _activeArtist?.Trim(); }
            set { SetValue(ref _activeArtist, value.Trim()); }
        }
        public string ActiveSong
        {
            get { return _activeSong?.Trim(); }
            set { SetValue(ref _activeSong, value.Trim()); }
        }
        public string PlayButton { get => _isPlaying ? "stop.png" : "play.png"; }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                SetValue(ref _isPlaying, value);
                OnPropertyChanged(nameof(PlayButton));
            }
        }
        public bool IsSignedIn
        {
            get { return _isSignedIn; }
            set
            {
                SetValue(ref _isSignedIn, value);
                OnPropertyChanged(nameof(AuthButton));
            }
        }
        public string Email
        {
            get 
            { 
                
                return _email?.Trim().Substring(0, _email.IndexOf('@')); 
            }
            set { SetValue(ref _email, value);
                OnPropertyChanged(nameof(EmailToName));
            }
        }
        public string EmailToName
        {
            get
            {
                if (_email == null)
                    return "Jack";
                TextInfo textinfo = new CultureInfo("en-US", false).TextInfo;
                var eersteDeel = _email?.Trim().Substring(0, _email.IndexOf('@'));
                if (eersteDeel.IndexOf('.') > -1)
                {
                    var voornaam = textinfo.ToTitleCase(eersteDeel.Substring(0, eersteDeel.IndexOf(".")));
                    return voornaam;
                }
                eersteDeel = textinfo.ToTitleCase(eersteDeel);
                return eersteDeel;
            }
        }
        public string AuthButton { get => IsSignedIn ? "logout.png" : "login.png"; }
        public CountryModel SelectedCountry
        {
            get => _selectedCountry;
            set => SetValue(ref _selectedCountry, value);
        }
        public List<Background> Colors
        {
            get => Backgrounds.GetColors();
        }
        public  Background SelectedBackground
        {
            get { return _selectedBackground; }
         
            set
            {
               _selectedBackground = value;
                Preferences.Set(Pref.background, SelectedBackground.Key.ToString());

            }

        }

        #endregion


        #region Commands
        public ICommand SearchCommand => new Command(SearchAction);
        public ICommand FilterTappedCommand => new Command(async _ => await ExecuteShowPopupCommand());
        public ICommand RefreshCommand => new Command(ExecuteRefreshCommand);
        public ICommand ShowPopupCommand => new Command(async _ => await ExecuteShowPopupCommand());
        public ICommand ChoicesSelectedCommand => new Command(_filterchoices => ExecuteFilterChoicesSelectedCommand(_filterchoices as FilterChoices));
        public ICommand StationSelectedCommand => new Command<Station>(stat => StationSelectedAsync(stat));
        public ICommand PlayCommand => new Command(async () => await PlayAsync());
        public ICommand AuthCommand => new Command(async _ => await CheckforAuthentication());
        public ICommand DoubleTappedCommand => new Command<Station>(async stat => await DoubleTappedAsync(stat));
        public ICommand ShowCountryPopupCommand => new Command(async _ => await ExecuteShowCountryPopupCommand());
        public ICommand CountrySelectedCommand => new Command(country => ExecuteCountrySelectedCommandAsync(country as CountryModel));
        public ICommand BackgroundCommand => new Command(ChangeBackgrounds);

        #endregion


        #region Methods
        /// <summary>
        /// Gets called when the app starts
        /// </summary>
        /// <returns></returns>
        private async Task InitializeAsync()
        {
            await GetStationsAsync();

            IsSignedIn = CheckIfSignedIn();
            OnPropertyChanged(nameof(IsSignedIn));

            _favourites = ConvertToCollection(SqlLiteService.GetFavourites().Result);
            OnPropertyChanged(nameof(FavouriteStations));
            // checks for meta data every 10 seconds
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                CheckForMetaAsync();

                return true; // True = Repeat again, False = Stop the timer
            });
        }
        private async Task GetStationsAsync()
        {
            // Get country
            _selectedCountry = CountryUtils.GetCountryModelByName(Preferences.Get(Pref.selectedCountry, "Belgium"));

            // Get chosen filters
            _filterChoices = new FilterChoices(Preferences.Get(Pref.selectedGenre, ""), Preferences.Get(Pref.selectedLanguage, ""));
            IsBusy = true;
            
            // Get stations from Api
            _unfilteredStations = await _darfmapi.GetStationsAsync(SelectedCountry.CountryCode.ToLower());

            // Filter them
            _filteredStations = Filter(_unfilteredStations, Preferences.Get(Pref.selectedGenre, ""), Preferences.Get(Pref.selectedLanguage, ""));

            // Change property
            FilteredStations = _filteredStations;
            OnPropertyChanged(nameof(FilteredStations));
            OnPropertyChanged(nameof(FavouriteStations));
        }


        #region StationsControl
        /// <summary>
        /// This gets called when station is selected
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        private async Task StationSelectedAsync(Station station)
        {

            if (station == null)
            {
                return;
            }
            if (station.IsSelected == true)
            {
                station.IsSelected = false;
                Preferences.Set(Pref.selectedStation, null);
                OnPropertyChanged(Preferences.Get(Pref.selectedStation, ""));
                _activeStation = null;
            }
            else
            {
                foreach (Station s in _filteredStations)
                {
                    s.IsSelected = false;
                }
                station.IsSelected = true;
                station.PlayUrl = null;
                station.PlayUrl = await DarFmApiStreaming.GetStreamAsync(station);
                ActiveStation = station;
                var item = await CrossMediaManager.Current.Extractor.CreateMediaItem(ActiveStation.PlayUrl);
                item.MediaType = MediaManager.Library.MediaType.Audio;
                await CrossMediaManager.Current.Play(item);
                IsPlaying = true;
                await CheckForMetaAsync();
                Preferences.Set(Pref.selectedStation, station.StationId);

            }
        }

        /// <summary>
        /// On Search
        /// </summary>
        void SearchAction()
        {
            RangedObservableCollection<Station> _searchFilteredStations;
            _filteredStations = Filter(_unfilteredStations, Preferences.Get(SelectedGenre, ""), Preferences.Get(SelectedLanguage, ""));
            if (string.IsNullOrEmpty(SearchText))
            {
                _searchFilteredStations = _filteredStations;
                _filteredStations = _searchFilteredStations;
            }
            else
            {
                _filteredStations = new RangedObservableCollection<Station>(from station in FilteredStations
                                                                            where station.Callsign.ToLower().Contains(SearchText.ToLower())
                                                                            select station);
            }
            FilteredStations = _filteredStations;
            OnPropertyChanged(nameof(FilteredStations));
        }

         /// <summary>
         /// pull to refresh, not sure if this works, implemented this in the very beginning
         /// </summary>
        async void ExecuteRefreshCommand()
        {
            if (IsRefreshing)
                return;
            IsRefreshing = true;

            FilteredStations.AddRange(await _darfmapi.GetStationsAsync(Preferences.Get(Pref.selectedCountry, "be")));
            //Stations.AddRange(await _apiService.GetStationsAsync(Preferences.Get(countryCode,"be"),offset));
            IsRefreshing = false;
        }


        /// <summary>
        /// Filter method to filter stations
        /// </summary>
        /// <param name="unfiltered"></param>
        /// <param name="genre"></param>
        /// <param name="language"></param>
        
        private RangedObservableCollection<Station> Filter(
            RangedObservableCollection<Station> unfiltered, string genre, string language)
        {

            if (genre == "")
            {
                _filteredbygenre = unfiltered;
            }
            else
            {
                _filteredbygenre = new RangedObservableCollection<Station>(from station in unfiltered
                                                                           where station.Genre.ToLower() == genre.ToLower()
                                                                           select station);
            }
            if (language == "")
            {
                _filteredbylanguage = _filteredbygenre;
            }
            else
            {
                _filteredbylanguage = new RangedObservableCollection<Station>(from station in _filteredbygenre
                                                                              where station.Language.ToLower() == language.ToLower()
                                                                              select station);
            }
            return _filteredbylanguage;

        }


        /// <summary>
        /// Called when filters are chosen
        /// </summary>
        /// <param name="filterChoices"></param>
        private void ExecuteFilterChoicesSelectedCommand(FilterChoices filterChoices)
        {
            ChoicesSelected = filterChoices;
        }

        /// <summary>
        /// Show popup
        /// </summary>
        /// <returns></returns>
        private Task ExecuteShowPopupCommand()
        {
            var popup = new FilterPopup(ChoicesSelected)
            {
                ChoicesSelectedCommand = ChoicesSelectedCommand
            };

            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }

        /// <summary>
        /// Converts a list to a RangedObservableCollection
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private RangedObservableCollection<Station> ConvertToCollection(List<string> ids)
        {
            RangedObservableCollection<Station> stations = new RangedObservableCollection<Station>();
            foreach (Station s in _unfilteredStations)
            {
                foreach (string id in ids)
                {
                    if (s.StationId == id)
                    {
                        stations.Add(s);
                    }
                }
            }
            return stations;
        }
        /// <summary>
        /// Doubletapping a Station adds it to favourites
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private async Task DoubleTappedAsync(Station s)
        {
            if (CheckIfIsFavourite(s.StationId))
            {
                await SqlLiteService.RemoveFavourite(s);
                _favourites.Remove(s);
            }
            else
            {
                await SqlLiteService.AddFavourite(s);
                _favourites.Add(s);
            }
        }

        
        #endregion

        #region MediaplayerControl
        /// <summary>
        /// Starts and stops playing the selected station
        /// </summary>
        /// <returns></returns>
        private async Task PlayAsync()
        {
            if (IsPlaying)
            {
                await CrossMediaManager.Current.Stop();
                IsPlaying = false;
            }
            else
            {
                if (ActiveStation != null)
                {
                    await CrossMediaManager.Current.Play(ActiveStation?.PlayUrl);
                    IsPlaying = true;
                }

            }
        }
        /// <summary>
        /// This gets called every 10 seconds to check if there's available metadata (artist and song)
        /// </summary>
        /// <returns></returns>
        private async Task CheckForMetaAsync()
        {
            MetaData metadata = await _darfmapi.GetCurrentlyPlayingAsync(ActiveStation);
            ActiveSong = metadata.Title;
            ActiveArtist = metadata.Artist;
            OnPropertyChanged(nameof(ActiveSong));
            OnPropertyChanged(nameof(ActiveArtist));
        }
        #endregion
        #region SettingsControl

        /// <summary>
        /// If signed in, sign out
        /// if signed out, sign in
        /// </summary>
        /// <returns></returns>
        private async Task CheckforAuthentication()
        {
            if (IsSignedIn)
            {
                Preferences.Remove(Pref.FirebaseToken);
                Preferences.Set(Pref.IsSignedIn, null);
                IsSignedIn = false;
                OnPropertyChanged(nameof(IsSignedIn));


            }
            else
            {
                await ShowAuthPopUp();
            }
        }
        /// <summary>
        /// Checks if a station is a favourite
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool CheckIfIsFavourite(string id)
        {
            bool b = false;
            if (id != null)
            {
                if (_favourites != null)
                {
                    foreach (Station stat in _favourites)
                    {
                        if (stat.StationId == id)
                        {
                            b = true;
                        }
                    }
                }

            }
            return b;
        }
        /// <summary>
        /// Checks if the user is signed in
        /// </summary>
        /// <returns></returns>
        private bool CheckIfSignedIn()
        {
            if (Preferences.Get(Pref.IsSignedIn, "") == Pref.True)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Show authentication popup
        /// </summary>
        /// <returns></returns>
        private Task ShowAuthPopUp()
        {
            var popup = new AuthPopup();
            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }

        /// <summary>
        /// Show Country popup
        /// </summary>
        /// <returns></returns>
        private Task ExecuteShowCountryPopupCommand()
        {
            var popup = new ChooseCountryPopup(SelectedCountry)
            {
                CountrySelectedCommand = CountrySelectedCommand
            };
            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }

        /// <summary>
        /// Select a country
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        private async Task ExecuteCountrySelectedCommandAsync(CountryModel country)
        {
            SelectedCountry = country;
            Preferences.Set(Pref.selectedCountry, country.CountryName);
            Preferences.Set(Pref.selectedLanguage, null);
            Preferences.Set(Pref.selectedGenre, null);
            _filterChoices = null;
            OnPropertyChanged(nameof(FilterChoices));
            await GetStationsAsync();
        }
        /// <summary>
        /// Because why not
        /// </summary>
        private void ChangeBackgrounds()
        {
            MessagingCenter.Send(this, "Background");
        }
        #endregion
        #endregion
    }
}
