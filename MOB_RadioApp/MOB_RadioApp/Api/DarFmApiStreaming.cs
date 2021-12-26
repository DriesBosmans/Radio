﻿using MOB_RadioApp.Models;
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
        public static async Task<string> GetStreamAsync(Station station)
        {
            string streamurl = "";
            string baseurl = "http://api.dar.fm/uberstationurl.php";
            string requesturl = baseurl + $"?station_id={station.StationId}&partnerToken=3360242197&callback=json";
            
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