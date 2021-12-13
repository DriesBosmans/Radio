﻿using MOB_RadioApp.Api;
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
            
            ShowPopupCommand = new Command(async _ => await ExecuteShowPopupCommand());
            _filterChoices = new FilterChoices(Preferences.Get(ProjectSettings.selectedGenre, ""), 
                Preferences.Get(ProjectSettings.selectedLanguage, ""));
            ChoicesSelectedCommand = new Command(_filterchoices => ExecuteFilterChoicesSelectedCommand(_filterchoices as FilterChoices));
            StationSelectedCommand = new Command<Station>(stat => StationSelected(stat));

            MessagingCenter.Subscribe<FilterPopup>(this, "x", (sender) =>
            {
                _filteredStations = Filter(_unfilteredStations, Preferences.Get(ProjectSettings.selectedGenre, ""), Preferences.Get(ProjectSettings.selectedLanguage, ""));
                FilteredStations = _filteredStations;
                OnPropertyChanged(nameof(FilteredStations));
            });
        }



        const string countryCode = "countrycode";
        ApiService _apiService = new ApiService();
        DarFmApiCall _darfmapi = new DarFmApiCall();
        public string SelectedGenre = Preferences.Get(ProjectSettings.selectedGenre, "");
        public string SelectedLanguage = Preferences.Get(ProjectSettings.selectedLanguage, "");
        private FilterChoices _filterChoices; 
        private RangedObservableCollection<Station> _filteredbygenre = new RangedObservableCollection<Station>();
        private RangedObservableCollection<Station> _filteredbylanguage = new RangedObservableCollection<Station>();
        private RangedObservableCollection<Station> _filteredStations;
        private RangedObservableCollection<Station> _unfilteredStations;
        private Station _activeStation;
        private string _activeImg;
        private string _activeCallsign;
        


        #region Properties

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetValue(ref _searchText, value); }
        }
        //filter selected
        public FilterChoices ChoicesSelected
        {
            get {  return _filterChoices; }
            set { SetValue(ref _filterChoices, value);}
        }
  
        public RangedObservableCollection<Station> FilteredStations
        {
            get { return _filteredStations; }
            set{ SetValue(ref _filteredStations, value); }
        }

        bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set { SetValue(ref isRefreshing, value); }
        }

        public Station ActiveStation
        {
            get { return _activeStation; }
            set { SetValue(ref _activeStation, value);
                OnPropertyChanged(nameof(ActiveImg));
            }
            
        }
        

        public string ActiveImg
        {
            get {  return ActiveStation?.Imageurl; }
            set { SetValue( ref _activeImg, value); }
        }
        
        public string ActiveCallSign
        {
            get { return ActiveStation?.Callsign; }
            set { SetValue( ref _activeCallsign, value); }
        }
        #endregion


        #region Commands
        public ICommand SearchCommand => new Command(SearchAction);
        public ICommand FilterTappedCommand => new Command(async _ => await ExecuteShowPopupCommand());
        public ICommand RefreshCommand => new Command(ExecuteRefreshCommand);
        public ICommand ShowPopupCommand { get; }
        public ICommand ChoicesSelectedCommand { get; }
//        public ICommand StationTappedCommand { get; }
        public ICommand StationSelectedCommand { get; }

        #endregion


        #region Methods

        //private void StationTapped()
        //{

        //    if (station.IsSelected == false)
        //    {
        //        station.IsSelected = true;
        //        Preferences.Set(ProjectSettings.selectedStation, station.StationId);

        //    }
        //    else
        //    {
        //        station.IsSelected = false;
        //    }
        //}
        private void StationSelected(Station station)
        {
            
            if (station == null)
            {
                return;
            }
            if (station.IsSelected == true)
            {
                station.IsSelected = false;
                Preferences.Set(ProjectSettings.selectedStation, null);
                OnPropertyChanged(Preferences.Get(ProjectSettings.selectedStation, ""));
                _activeStation = null;
            }
            else
            {
                foreach (Station s in _filteredStations)
                {
                    s.IsSelected = false;
                }
                station.IsSelected = true;
                station.PlayUrl = DarFmApiStreaming.GetFuckingStreamAsync(station).Result;
                ActiveStation = station;
                
                Preferences.Set(ProjectSettings.selectedStation, station.StationId);
                        
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

        private async Task InitializeAsync()
        {
            IsBusy = true;
            _unfilteredStations = await _darfmapi.GetStationsAsync(await GetCountry());
            _filteredStations = Filter(_unfilteredStations, Preferences.Get(ProjectSettings.selectedGenre, ""), Preferences.Get(ProjectSettings.selectedLanguage, ""));

            FilteredStations = _filteredStations;
            OnPropertyChanged(nameof(FilteredStations));
            //if (Preferences.Get(ProjectSettings.selectedStation, "") != null)
            //{
            //    ActiveStation = GetStation(Preferences.Get(ProjectSettings.selectedStation, ""));
            //}
            //else
            //    ActiveStation = null;
            //IsBusy = false;
        }

        private RangedObservableCollection<Station> Filter(
            RangedObservableCollection<Station> unfiltered, string genre, string language)
        {
            
            if(genre == "")
            {
                _filteredbygenre = unfiltered;
            }
            else
            {
                _filteredbygenre = new RangedObservableCollection<Station>(from station in unfiltered
                                                                where station.Genre.ToLower() == genre.ToLower()
                                                                select station);
            }
            if(language == "")
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
        private Station GetStation(string id)
        {if (id != null)
            {
                foreach (Station s in AllStations.Stations)
                {
                    if (id == s.StationId)
                    {
                        return s;
                    }
                }
                return null;
            }
            return null;
        }
        #endregion
    }
}
