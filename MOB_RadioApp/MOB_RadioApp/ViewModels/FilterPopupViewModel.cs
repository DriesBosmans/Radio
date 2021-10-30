using MOB_RadioApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace MOB_RadioApp.ViewModels
{
    public class FilterPopupViewModel : BaseViewModel2
    {
        #region Fields
        private ObservableCollection<string> _availableGenres;
        private ObservableCollection<string> _availableLanguages;
        #endregion Fields
        #region Constructors
        public FilterPopupViewModel()
        {

        }
        #endregion Constructors
        #region Properties
        public ObservableCollection<string> AvailableGenres
        {
            get
            {
                _availableGenres = new ObservableCollection<string>();  
                foreach (Station station in AllStations.Stations)
                {
                    if (!_availableGenres.Contains(station.Genre))
                    {
                        _availableGenres.Add(station.Genre);
                    }
                }
                return _availableGenres;
            }

        }
        public ObservableCollection<string> AvailableLanguages
        {
            get
            {
                _availableLanguages = new ObservableCollection<string>();
                foreach (Station station in AllStations.Stations)
                {
                    if (!_availableLanguages.Contains(station.Language))
                    {
                        _availableLanguages.Add(station.Language);
                    }
                }
                return _availableLanguages;

            }
            #region Commands

            #endregion Commands
            #region Private Methods

            #endregion Private Methods
        }

        #endregion Properties

    }
}
