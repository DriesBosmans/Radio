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

namespace MOB_RadioApp.Api
{
    public class DarFmApiCall
    {
        RestClient _client;
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
        //public async Task<string> GetPlayUrlAsync(Station station)
        //{
        //    var playurl = "";
        //    string url = streamingUrl + param +
        //        $"station_id={station.StationId}" + and +
        //        partnerToken + and + "callback=json";

        //    var client = new HttpClient();
        //    var request = new HttpRequestMessage
        //    {
        //        Method = HttpMethod.Get,
        //        RequestUri = new Uri(url)
        //    };
        //    HttpResponseMessage response = await client.SendAsync(request);
        //    //{
        //    //    response.EnsureSuccessStatusCode();
        //    //    var body = await response.Content.ReadAsStringAsync();
        //    //}


        //    //try
        //    //{
        //    //    IRestResponse response = await _client.ExecuteAsync(req);
        //    //    if (response.IsSuccessful)
        //    //    {
        //    //        var json = response.Content
        //    //            .Substring(response.Content.Length - 1, 1)
        //    //            .Substring(0, 16);
        //    //        Streamurl stream = JsonConvert.DeserializeObject<Streamurl>(json);

        //    //    }

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    playurl = null;
        //    //}
        //    //playurl = url;
        //    return playurl;
        //}
    }
}
