using MOB_RadioApp.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MOB_RadioApp.Api
{
    public class DarFmApiStreaming
    {
        /// <summary>
        /// This file get the streaming URLs from the API
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public static async Task<string> GetStreamAsync(Station station)
        {
            string streamurl = "";
            string baseurl = "http://api.dar.fm/uberstationurl.php";
            string requesturl = baseurl + $"?station_id={station.StationId}&partnerToken=" + ProjectSettings.Token + "&callback=json";

            HttpClient httpClient = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requesturl)
            };
            try
            {
                HttpResponseMessage response = await httpClient.SendAsync(request);
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    RootStream myDeserializedClass = JsonConvert.DeserializeObject<RootStream>(body);
                    ResultStream resultStream = myDeserializedClass.Result[0];
                    streamurl = resultStream.Url;

                }
            }
            catch (Exception ex) { }

            return streamurl;
        }
    }
}
