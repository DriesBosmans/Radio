using MediaManager;
using MOB_RadioApp.Api;
using MOB_RadioApp.Models;
using MOB_RadioApp.Popups;
using MOB_RadioApp.Services;
using MvvmHelpers;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

    public class MainViewModel : BaseViewModel2
    {
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

        const string countryCode = "countrycode";
        GetCountryCode _apiService = new GetCountryCode();
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
        private string _activeCallsign;
        private bool _isPlaying = false;
        private string _activeSong;
        private string _activeArtist;
        private string _searchText;
        private bool _isSignedIn;
        private bool isRefreshing;
        public string _email;
        private CountryModel _selectedCountry;
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
            set { SetValue(ref _activeArtist, value); }
        }
        public string ActiveSong
        {
            get { return _activeSong?.Trim(); }
            set { SetValue(ref _activeSong, value); }
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
            get { return _email?.Trim().Substring(0, _email.IndexOf('@')); }
            set { SetValue(ref _email, value); }
        }
        public string AuthButton { get => IsSignedIn ? "logout.png" : "login.png"; }
        public CountryModel SelectedCountry
        {
            get => _selectedCountry;
            set => SetValue(ref _selectedCountry, value);
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
        public ICommand AuthCommand => new Command(async _ => await CheckAuthAsync());
        public ICommand DoubleTappedCommand => new Command<Station>(stat => DoubleTappedAsync(stat));
        public ICommand ShowCountryPopupCommand => new Command(async _ => await ExecuteShowCountryPopupCommand());
        public ICommand CountrySelectedCommand => new Command(country => ExecuteCountrySelectedCommand(country as CountryModel));

        #endregion


        #region Methods
        /// <summary>
        /// Gets called when the app starts
        /// </summary>
        /// <returns></returns>
        private async Task InitializeAsync()
        {
            SelectedCountry = CountryUtils.GetCountryModelByName("United States");
          
            _filterChoices = new FilterChoices(Preferences.Get(Pref.selectedGenre, ""),
                Preferences.Get(Pref.selectedLanguage, ""));
            IsBusy = true;
            _unfilteredStations = await _darfmapi.GetStationsAsync(await GetCountry());
            _filteredStations = Filter(_unfilteredStations, Preferences.Get(Pref.selectedGenre, ""),
                Preferences.Get(Pref.selectedLanguage, ""));

            FilteredStations = _filteredStations;
            OnPropertyChanged(nameof(FilteredStations));

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
        #region StationsControl

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


        async void ExecuteRefreshCommand()
        {
            if (IsRefreshing)
                return;
            IsRefreshing = true;

            FilteredStations.AddRange(await _darfmapi.GetStationsAsync(Preferences.Get(countryCode, "be")));
            //Stations.AddRange(await _apiService.GetStationsAsync(Preferences.Get(countryCode,"be"),offset));
            IsRefreshing = false;
        }



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


        private async Task<string> GetCountry()
        {
            //var currentRegion = RegionInfo.CurrentRegion.TwoLetterISORegionName;
            var currentRegion = Pref.Region;
            //if (currentRegion != Preferences.Get(countryCode, ""))
            //{
            //    var countries = await _apiService.GetCountriesAsync();
            //    var enumCountries = countries.AsEnumerable<CountryModel>();
            //    var results = from c in enumCountries
            //                  where c.Name.ToUpper() == currentRegion.ToUpper()
            //                  select c;
            //    if (results.Count() == 1)
            //    {
            //        Preferences.Set(countryCode, currentRegion);
            //        return currentRegion;
            //    }
            //    else
            //    {
            //        Preferences.Set(countryCode, currentRegion);
            //        return currentRegion;
            //    }
            //}
            return currentRegion = "be";
        }

        private void ExecuteFilterChoicesSelectedCommand(FilterChoices filterChoices)
        {
            ChoicesSelected = filterChoices;
        }

        private Task ExecuteShowPopupCommand()
        {
            var popup = new FilterPopup(ChoicesSelected)
            {
                ChoicesSelectedCommand = ChoicesSelectedCommand
            };

            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }
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

        private Task ExecuteShowCountryPopupCommand()
        {
            var popup = new ChooseCountryPopup(SelectedCountry)
            {
                CountrySelectedCommand = CountrySelectedCommand
            };
            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }

        private void ExecuteCountrySelectedCommand(CountryModel country)
        {
            SelectedCountry = country;
        }
        #endregion
        #region MediaplayerControl
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
        private async Task CheckAuthAsync()
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
                await ShowPopup();
            }
        }
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
        private bool CheckIfSignedIn()
        {
            if (Preferences.Get(Pref.IsSignedIn, "") == Pref.True)
            {
                return true;
            }
            else
                return false;
        }
        private Task ShowPopup()
        {
            var popup = new AuthPopup();
            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }

        #endregion
        #endregion
    }
}
