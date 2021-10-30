using MOB_RadioApp.Api;
using MOB_RadioApp.Models;
using MOB_RadioApp.Popups;
using MOB_RadioApp.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MOB_RadioApp.ViewModels
{

    public class MainViewModel : BaseViewModel2
    {
        public MainViewModel()
        {
            InitializeAsync();
            //Navigation = navigation;
            ShowPopupCommand = new Command(async _ => await ExecuteShowPopupCommand());
          
        }



        const string countryCode = "countrycode";
        ApiService _apiService = new ApiService();
        DarFmApiCall _darfmapi = new DarFmApiCall();
        private readonly INavigation Navigation;
        public string SelectedGenre = Preferences.Get(ProjectSettings.selectedGenre, "");
        public string SelectedLanguage = Preferences.Get(ProjectSettings.selectedLanguage, "");

        

        #region Properties

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetValue(ref _searchText, value); }
        }

        private RangedObservableCollection<Station> _unfilteredStations;
        private RangedObservableCollection<Station> _filteredStations;
        private RangedObservableCollection<Station> _stations;
        public RangedObservableCollection<Station> Stations
        {
            get { return _stations; }
            set{ SetValue(ref _stations, value); }
        }

        bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                SetValue(ref isRefreshing, value);
            }
        }
        #endregion


        #region Commands
        public ICommand SearchCommand => new Command(SearchAction);
        public ICommand FilterTappedCommand => new Command(async _ => await ExecuteShowPopupCommand());
        public ICommand RefreshCommand => new Command(ExecuteRefreshCommand);
        public ICommand ShowPopupCommand { get; }
        public ICommand GenreSelectedCommand { get; }
        #endregion


        #region Methods

        void SearchAction()
        {
            if (string.IsNullOrEmpty(this._searchText))
            {
                Stations = _unfilteredStations ;
            }
            else
            {
                _filteredStations = new RangedObservableCollection<Station>(from station in Stations
                                                                            where station.Callsign.ToLower().Contains(SearchText.ToLower()) select station);
                Stations = _filteredStations;
            }
        }

        void FilterTappedAction()
        {
         
        }
        async void ExecuteRefreshCommand()
        {
            if (IsRefreshing)
                return;
            IsRefreshing = true;

            Stations.AddRange(await _darfmapi.GetStationsAsync(Preferences.Get(countryCode, "be")));
            //Stations.AddRange(await _apiService.GetStationsAsync(Preferences.Get(countryCode,"be"),offset));
            IsRefreshing = false;
        }

        private async Task InitializeAsync()
        {
            IsBusy = true;
            Stations = await _darfmapi.GetStationsAsync(await GetCountry());
            OnPropertyChanged(nameof(Stations));
            _unfilteredStations = _stations;
            IsBusy = false;
        }

        private async Task<string> GetCountry()
        {
            //var currentRegion = RegionInfo.CurrentRegion.TwoLetterISORegionName;
            var currentRegion = ProjectSettings.Region;
            if (currentRegion != Preferences.Get(countryCode, ""))
            {
                var countries = await _apiService.GetCountriesAsync();
                var enumCountries = countries.AsEnumerable<Country>();
                var results = from c in enumCountries
                              where c.Name.ToUpper() == currentRegion.ToUpper()
                              select c;
                if (results.Count() == 1)
                {
                    Preferences.Set(countryCode, currentRegion);
                    return currentRegion;
                }
                else
                {
                    Preferences.Set(countryCode, currentRegion);
                    return currentRegion;
                }
            }
            return currentRegion = "be";
        }

        private Task ExecuteShowPopupCommand()
        {
            var popup = new FilterPopup();
         
            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }
      
        #endregion
    }
}
