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
            var request = new RestRequest(Method.GET);
            IRestResponse response = await _client.ExecuteAsync(request);
            if(response.IsSuccessful)
            {
                deserializedObject = JsonConvert.DeserializeObject<Root>(response.Content);
                deserializedObject.Result[0].Stations.ForEach(x => stations.Add(x));
            }
            AllStations.Stations = stations;
            return stations;
        }
    }
}
