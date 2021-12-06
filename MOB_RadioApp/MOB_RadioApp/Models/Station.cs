using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Serialization;

namespace MOB_RadioApp.Models
{
    public static class AllStations
    {
        public static RangedObservableCollection<Station> Stations { get; set;  }
    }
    public class Station : StationProps
    {
        [JsonProperty("band")]
        public string Band { get; set; }

        [JsonProperty("genre")]
        public string Genre { get; set; }

        [JsonProperty("ubergenre")]
        public string Ubergenre { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("websiteurl")]
        public string Websiteurl { get; set; }

        private string _imageUrl;
        [JsonProperty("imageurl")]
        public string Imageurl
        {
            get { return _imageUrl; }
            set { SetValue(ref _imageUrl, value);
                OnPropertyChanged(nameof(Imageurl));
                    }
        }
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("encoding")]
        public string Encoding { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("zipcode")]
        public string Zipcode { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("address1")]
        public string Address1 { get; set; }

        [JsonProperty("address2")]
        public string Address2 { get; set; }

        [JsonProperty("callsign")]
        public string Callsign { get; set; }

        [JsonProperty("dial")]
        public string Dial { get; set; }

        [JsonProperty("station_id")]
        public string StationId { get; set; }

        [JsonProperty("station_image")]
        public string StationImage { get; set; }

        [JsonProperty("slogan")]
        public string Slogan { get; set; }
    }

    public class Result
    {
        [JsonProperty("zipcode")]
        public string Zipcode { get; set; }

        [JsonProperty("city")]
        public object City { get; set; }

        [JsonProperty("state")]
        public object State { get; set; }

        [JsonProperty("stations")]
        public List<Station> Stations { get; set; }
    }

    public class Root
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public List<Result> Result { get; set; }
    }




}
