using MOB_RadioApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace MOB_RadioApp.ViewModels
{
    public class MediaplayerViewModel : BaseViewModel2
    {
        #region Fields
        private string _stationPlayingId = ProjectSettings.selectedStation;
        
       
      
        #endregion Fields
        #region Constructors

        #endregion Constructors
        #region Properties
        public string StationPlayingId 
        { 
            get { return _stationPlayingId; }
            set
            {
                SetValue(ref _stationPlayingId, value);
                OnPropertyChanged(nameof(StationPlayingId));
                OnPropertyChanged(nameof(StationPlaying));
            }
        }
        public Station StationPlaying 
        {
            get
            {
                if (AllStations.Stations != null)
                {
                    foreach (Station s in AllStations.Stations)
                    {
                        if (s.StationId == _stationPlayingId)
                            return s;
                    }
                    return null;
                }
                else return null;
            }
           
        }
        
        #endregion Properties
        #region Commands

        #endregion Commands
        #region Private Methods

        #endregion Private Methods
    }
}
