using MOB_RadioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace MOB_RadioApp.Models
{
  
    public static class OldAllStations
    {
        public static ObservableCollection<OldStation> oldstations { get; set; }
        
    }
    public class OldStation : BaseViewModel2
    {
        public string Changeuuid { get; set;  }
        public string Stationuuid { get; set; }
        public string Serveruuid { get; set; }
        private string _name;
        public string Name {
            get 
            {
                return _name;
            }
            set {
                SetValue(ref _name, value.Trim());
            } }
        public string Url { get; set; }
        public string UrlResolved { get; set; }
        public string Homepage { get; set; }

        private string _favicon;
        public string Favicon
        {
            get
            {
                return _favicon;
            }
            set
            {
                if (value == "") _favicon = "radio_96.png";
                else _favicon = value;
            }
        }
        public string Tags { get; set; }
        public string Country { get; set; }
        public string Countrycode { get; set; }
        public object Iso31662 { get; set; }
        public string State { get; set; }
        public string Language { get; set; }
        public string Languagecodes { get; set; }
        public int Votes { get; set; }
        public string Lastchangetime { get; set; }
        public DateTime LastchangetimeIso8601 { get; set; }
        public string Codec { get; set; }
        public int Bitrate { get; set; }
        public int Hls { get; set; }
        public int Lastcheckok { get; set; }
        public string Lastchecktime { get; set; }
        public DateTime LastchecktimeIso8601 { get; set; }
        public string Lastcheckoktime { get; set; }
        public DateTime LastcheckoktimeIso8601 { get; set; }
        public string Lastlocalchecktime { get; set; }
        public DateTime LastlocalchecktimeIso8601 { get; set; }
        public string Clicktimestamp { get; set; }
        public DateTime ClicktimestampIso8601 { get; set; }
        public int Clickcount { get; set; }
        public int Clicktrend { get; set; }
        public int SslError { get; set; }
        public double? GeoLat { get; set; }
        public double? GeoLong { get; set; }
        public bool HasExtendedInfo { get; set; }

    }
}
