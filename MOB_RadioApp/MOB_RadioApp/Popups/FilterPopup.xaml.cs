﻿using MOB_RadioApp.Models;
using MOB_RadioApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MOB_RadioApp.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPopup : PopupPage
    {
       
        
        #region Fields
        private ObservableCollection<string> _availableGenres;
        private ObservableCollection<string> _availableLanguages;
        private FilterChoices _selectedFilterChoices;
        #endregion Fields
        #region Constructors
        public FilterPopup(FilterChoices selectedChoices)
        {
            InitializeComponent();
            BindingContext = this;
            if (_availableGenres == null || !_availableGenres.Any())
                LoadGenres();
            if (_availableLanguages == null || !_availableLanguages.Any())
                LoadLanguages();
            AvailableGenres = new ObservableCollection<string>(_availableGenres);
            AvailableLanguages = new ObservableCollection<string>(_availableLanguages);
            PckrGenre.ItemsSource = AvailableGenres;
            PckrLanguage.ItemsSource = AvailableLanguages;
           _selectedFilterChoices = selectedChoices;
            
            if (selectedChoices.FilterGenre == "")
                PckrGenre.SelectedIndex = 0;
            else PckrGenre.SelectedIndex = AvailableGenres.IndexOf(selectedChoices.FilterGenre);
            
            if (selectedChoices.FilterLanguage == "")
                PckrLanguage.SelectedIndex = 0;
            else PckrLanguage.SelectedIndex = AvailableLanguages.IndexOf(selectedChoices.FilterLanguage);
           
            

        }
        //public FilterPopupViewModel()
        //{
        //    ClosePopupCommand = new Command(async _ => await ExecuteClosePopupCommand());
        //}
        #endregion Constructors

        #region Properties
        public ObservableCollection<string> AvailableGenres { get; }
        //{
        //    get
        //    {
        //        _availableGenres = new ObservableCollection<string>();
        //        foreach (Station station in AllStations.Stations)
        //        {
        //            if (!_availableGenres.Contains(station.Genre))
        //            {
        //                _availableGenres.Add(station.Genre);
        //            }
        //        }
        //        return _availableGenres;
        //    }

        //}
        public ObservableCollection<string> AvailableLanguages { get; }
        //{
        //    get
        //    {
        //        _availableLanguages = new ObservableCollection<string>();
        //        foreach (Station station in AllStations.Stations)
        //        {
        //            if (!_availableLanguages.Contains(station.Language))
        //            {
        //                _availableLanguages.Add(station.Language);
        //            }
        //        }
        //        return _availableLanguages;

        //    }
        public FilterChoices SelectedFilterChoices
        {
            get => _selectedFilterChoices;
            set
            {
                _selectedFilterChoices = value;
                OnPropertyChanged(nameof(SelectedFilterChoices));
            }
        }
            #region Commands
        public ICommand ChoisesSelectedCommand { get; set; }
        #endregion Commands

        #region Private Methods

        #endregion Private Methods

 

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {
            Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }

        private void BtnConfirm_Clicked(object sender, EventArgs e)
        {
            ChoisesSelectedCommand?.Execute(SelectedFilterChoices);
            Preferences.Set(ProjectSettings.selectedGenre, _selectedFilterChoices.FilterGenre);
           
            Preferences.Set(ProjectSettings.selectedLanguage, _selectedFilterChoices.FilterLanguage);
            MessagingCenter.Send(this, "x");
            Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
        private void LoadGenres()
        {
            _availableGenres = new ObservableCollection<string>();
            _availableGenres.Add("-");
            foreach (Station station in AllStations.Stations)
            {
                if (!_availableGenres.Contains(station.Genre))
                {
                    _availableGenres.Add(station.Genre);
                }
            }
   
        }
        private void LoadLanguages()
        {

            _availableLanguages = new ObservableCollection<string>();
            _availableLanguages.Add("-");
            foreach (Station station in AllStations.Stations)
            {
                if (!_availableLanguages.Contains(station.Language))
                {
                    _availableLanguages.Add(station.Language);
                }
            }
        }

        private void PckrGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PckrGenre.SelectedIndex == 0)
                _selectedFilterChoices.FilterGenre = "";

            else
                _selectedFilterChoices.FilterGenre = AvailableGenres[PckrGenre.SelectedIndex];
        }

        private void PckrLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PckrLanguage.SelectedIndex == 0)
                _selectedFilterChoices.FilterLanguage = "";
            else
                _selectedFilterChoices.FilterLanguage = AvailableLanguages[PckrLanguage.SelectedIndex];
        }
    }
    

        #endregion Properties
        #region Private Methods
        //private Task ExecuteClosePopupCommand()
        //{
        //    Preferences.Set(ProjectSettings.selectedGenre, SelectedGenre);
        //    Preferences.Set(ProjectSettings.selectedLanguage, SelectedLanguage);
        //    return Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        //}
        #endregion Private Methods
        #region Fields

        #endregion Fields

        #region Properties

        #endregion Properties
        #region Commands

        #endregion Commands
        #region Private Methods

        #endregion Private Methods
    
}