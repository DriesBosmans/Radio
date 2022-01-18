using MOB_RadioApp.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using static MOB_RadioApp.Models.MetaInfo;

namespace MOB_RadioApp.Api
{
    /// <summary>
    /// Here api calls are made to DarFM to get the available stations
    /// </summary>
    public class DarFmApiCall
    {
        RestClient _client;
        HttpClient _httpClient = new HttpClient();
        private const string baseUrl = "http://api.dar.fm/darstations.php";
        private const string playlistUrl = "http://api.dar.fm/playlist.php";
        private const string partnerToken = "partner_token=";
        private const string param = "?";
        private const string and = "&";
        private const string callback = "callback=json";
        private string exact = "exact=1";
        /// <summary>
        /// Finds stations based on the selected country
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public async Task<RangedObservableCollection<Station>> GetStationsAsync(string country)
        {
            RangedObservableCollection<Station> stations = new RangedObservableCollection<Station>();
            string countrystring = $"country={country}";
            string url = baseUrl + param +
                callback + and +
                countrystring + and +
                exact + and +
                partnerToken + ProjectSettings.Token;
            _client = new RestClient(url);
            Root deserializedObject;
            
            RestRequest request = new RestRequest { Method = Method.Get };
            try
            {
                var response = await _client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    deserializedObject = JsonConvert.DeserializeObject<Root>(response.Content);
                    foreach (Station station in deserializedObject.Result[0].Stations)
                    {
                        if (station.Imageurl != "")
                        {

                            if (station.Callsign != "")
                            {
                                stations.Add(station);
                            }

                        }
                    }

                }
                AllStations.Stations = stations;
            }
            catch (Exception ex)
            {
                url = null;
            }

            return stations;
        }
        /// <summary>
        /// Get metadata (what song is currently playing. This method gets called ever 15 seconds
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public async Task<MetaData> GetCurrentlyPlayingAsync(Station station)
        {
            MetaData metaData = new MetaData();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,

                RequestUri = new Uri(playlistUrl + $"?station_id={station.StationId}" + and +
             partnerToken + ProjectSettings.Token + "&callback=json")
            };
            try
            {
                HttpResponseMessage response = await _httpClient.SendAsync(request);
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    RootMeta rootmeta = JsonConvert.DeserializeObject<RootMeta>(body);
                    metaData = rootmeta.MetaDatas[0];
                }
            }
            catch (Exception ex) { }
            return metaData;
        }
    }
}
