using MOB_RadioApp.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static MOB_RadioApp.Models.MetaInfo;

namespace MOB_RadioApp.Api
{
    public class DarFmApiCall
    {
        RestClient _client;
        HttpClient _httpClient = new HttpClient();
        private const string baseUrl = "http://api.dar.fm/darstations.php";
        private const string streamingUrl = "http://api.dar.fm/uberstationurl.php";
        private const string partnerToken = "partner_token=3360242197";
        private const string param = "?";
        private const string and = "&";
        private const string callback = "callback=json";
        private string exact = "exact=1";
        
        public async Task<RangedObservableCollection<Station>> GetStationsAsync(string country)
        {
            RangedObservableCollection<Station> stations = new RangedObservableCollection<Station>();
            string countrystring = $"country={country}";
            string url = baseUrl + param + 
                callback + and + 
                countrystring + and +
                exact + and +
                partnerToken;
            _client = new RestClient(url);
            Root deserializedObject;
            RestRequest request = new RestRequest(Method.GET);
            try { IRestResponse response = await _client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    deserializedObject = JsonConvert.DeserializeObject<Root>(response.Content);
                    foreach(Station station in deserializedObject.Result[0].Stations)
                    {
                        if(station.Imageurl != "")
                        {
                            
                                if(station.Callsign != "")
                                {
                                    stations.Add(station);
                                }
                            
                        }
                    }
                      
                }
                AllStations.Stations = stations;
            }
            catch (Exception ex){
                url = null;
            }
            
            return stations;
        }
        public async Task<MetaData> GetCurrentlyPlayingAsync(Station station)
        {
            MetaData metaData = new MetaData();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,

                RequestUri = new Uri(baseUrl + $"/playlist.php?station_id={station.StationId}" +
            $"&partner_token=" + partnerToken + "&callback=json")
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
            } catch (Exception ex) { }
            return metaData;
        }
    }
}
