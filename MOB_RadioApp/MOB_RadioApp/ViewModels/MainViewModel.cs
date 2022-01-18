using LibVLCSharp.Shared;
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
                _filteredStations = Filter(_unfilteredStations, Preferences.Get(ProjectSettings.selectedGenre, ""), Preferences.Get(ProjectSettings.selectedLanguage, ""));
                FilteredStations = _filteredStations;
                OnPropertyChanged(nameof(FilteredStations));
            });
            // This lets this viewmodel know who has logged in and when
            MessagingCenter.Subscribe<AuthPopup, string>(this, ProjectSettings.Email, (sender, arg) =>
            {
                Email = arg;
                Preferences.Set(ProjectSettings.Email, arg);
                OnPropertyChanged(nameof(Email)); IsSignedIn = true;
                Preferences.Set(ProjectSettings.IsSignedIn, ProjectSettings.True);
                OnPropertyChanged(nameof(IsSignedIn));
                _favourites = ConvertToCollection(SqlLiteService.GetFavourites().Result);

                OnPropertyChanged(nameof(FavouriteStations));

            });
        }

        #region Fields
        DarFmApiCall _darfmapi = new DarFmApiCall();
        public string SelectedGenre = Preferences.Get(ProjectSettings.selectedGenre, "");
        public string SelectedLanguage = Preferences.Get(ProjectSettings.selectedLanguage, "");
        private FilterChoices _filterChoices;
        private RangedObservableCollection<Station> _filteredbygenre = new RangedObservableCollection<Station>();
        private RangedObservableCollection<Station> _filteredbylanguage = new RangedObservableCollection<Station>();
        private RangedObservableCollection<Station> _filteredStations;
        private RangedObservableCollection<Station> _unfilteredStations;
        private RangedObservableCollection<Station> _favourites = new RangedObservableCollection<Station>();
        private Station _activeStation;
        private string _activeImg;
        private LibVLC _libVLC;
        private MediaPlayer _player;
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
        public LibVLC LibVLC
        {
            get { return _libVLC; }
            set { SetValue(ref _libVLC, value); }
        }
        public MediaPlayer MediaPlayer
        {
            get { return _player; }
            set { SetValue(ref _player, value); }
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
            set
            {
                SetValue(ref _email, value);
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
        public Background SelectedBackground
        {
            get { return _selectedBackground; }

            set
            {
                _selectedBackground = value;
                Preferences.Set(ProjectSettings.background, SelectedBackground.Key.ToString());

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
        public ICommand PlayCommand => new Command(() => PlayAsync());
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
            _email = Preferences.Get(ProjectSettings.Email, "");
            OnPropertyChanged(nameof(EmailToName));

            _favourites = ConvertToCollection(SqlLiteService.GetFavourites().Result);
            OnPropertyChanged(nameof(FavouriteStations));
            // checks for meta data every 10 seconds
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                CheckForMetaAsync();

                return true; // True = Repeat again, False = Stop the timer
            });
            Core.Initialize();


        }
        private async Task GetStationsAsync()
        {
            // Get country
            _selectedCountry = CountryUtils.GetCountryModelByName(Preferences.Get(ProjectSettings.selectedCountry, "Belgium"));

            // Get chosen filters
            _filterChoices = new FilterChoices(Preferences.Get(ProjectSettings.selectedGenre, ""), Preferences.Get(ProjectSettings.selectedLanguage, ""));
            IsBusy = true;

            // Get stations from Api
            _unfilteredStations = await _darfmapi.GetStationsAsync(SelectedCountry.CountryCode.ToLower());

            // Filter them
            _filteredStations = Filter(_unfilteredStations, Preferences.Get(ProjectSettings.selectedGenre, ""), Preferences.Get(ProjectSettings.selectedLanguage, ""));

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

            if (station.IsSelected == true)
            {
                station.IsSelected = false;
                Preferences.Set(ProjectSettings.selectedStation, null);
                OnPropertyChanged(Preferences.Get(ProjectSettings.selectedStation, ""));
                MediaPlayer.Stop();
                LibVLC.Dispose();
                IsPlaying = false;
                OnPropertyChanged(nameof(IsPlaying));
                ActiveSong = null;
                ActiveImg = null;
                ActiveArtist = null;
                _activeStation = null;
                OnPropertyChanged(nameof(ActiveStation));
                OnPropertyChanged(nameof(ActiveSong));
                OnPropertyChanged(nameof(ActiveImg));
                OnPropertyChanged(nameof(ActiveArtist));

            }
            else
            {
                foreach (Station s in _filteredStations)
                {
                    s.IsSelected = false;
                }
                if (MediaPlayer != null)
                    if (MediaPlayer.IsPlaying)
                        MediaPlayer.Stop();

                station.IsSelected = true;
                station.PlayUrl = null;
                station.PlayUrl = await DarFmApiStreaming.GetStreamAsync(station);
                ActiveStation = station;
                LibVLC = new LibVLC();
                var media = new Media(LibVLC, station.PlayUrl, FromType.FromLocation);
                MediaPlayer = new MediaPlayer(media) { EnableHardwareDecoding = true };
                MediaPlayer.Buffering += MediaPlayer_Buffering;
                MediaPlayer.Play();
                IsPlaying = true;
                await CheckForMetaAsync();
                Preferences.Set(ProjectSettings.selectedStation, station.StationId);

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

            FilteredStations.AddRange(await _darfmapi.GetStationsAsync(Preferences.Get(ProjectSettings.selectedCountry, "be")));
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
        private void PlayAsync()
        {
            if (MediaPlayer.IsPlaying)
            {
                MediaPlayer.Stop();
                IsPlaying = false;
            }
            else
            {
                if (ActiveStation != null)
                { 
                    MediaPlayer.Play();
                    
                    IsPlaying = true;
                }
                else
                {
                    //await App.Current.MainPage.DisplayAlert("Foutje", "U heeft geen station meer geselecteerd", "Oké");
                }

            }
        }

        private void MediaPlayer_Buffering(object sender, MediaPlayerBufferingEventArgs e)
        {

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
                Preferences.Remove(ProjectSettings.FirebaseToken);
                Preferences.Set(ProjectSettings.IsSignedIn, null);
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
            if (Preferences.Get(ProjectSettings.IsSignedIn, "") == ProjectSettings.True)
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
            Preferences.Set(ProjectSettings.selectedCountry, country.CountryName);
            Preferences.Set(ProjectSettings.selectedLanguage, null);
            Preferences.Set(ProjectSettings.selectedGenre, null);
            _filterChoices = null;
            OnPropertyChanged(nameof(FilterChoices));
            await GetStationsAsync();
        }
        /// <summary>
        /// Because why not
        /// </summary>
        private void ChangeBackgrounds()
        {
            MessagingCenter.Send(this, ProjectSettings.background);
            if (Preferences.Get(ProjectSettings.background, "") == 8.ToString())
                MessagingCenter.Send(this, "askew");
            if (Preferences.Get(ProjectSettings.background, "") != 8.ToString())
                MessagingCenter.Send(this, "straight");

        }
        #endregion
        #endregion
    }
}
