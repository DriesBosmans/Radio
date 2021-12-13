using MOB_RadioApp.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MOB_RadioApp.Api
{
    public static class DarFmApiStreaming
    {
        public static async Task<string> GetFuckingStreamAsync(Station station)
        {
            string streamurl = "";
            string baseurl = "http://api.dar.fm/uberstationurl.php";
            var client = new RestClient(baseurl + $"?station_id={station.StationId}&partnerToken=3360242197&callback=json");
            RestRequest restRequest = new RestRequest(Method.GET);
            IRestResponse restResponse = await client.ExecuteAsync(restRequest);
            var json = restRequest.Body;
            

            return streamurl ;
        }
    }
}
