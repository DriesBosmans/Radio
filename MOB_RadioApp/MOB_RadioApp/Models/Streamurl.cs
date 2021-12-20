using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.Core;

namespace MOB_RadioApp.Models
{
   
    public class ResultStream
    {
        [JsonProperty("url")]
        public string Url;

        [JsonProperty("encoding")]
        public string Encoding;

        [JsonProperty("callsign")]
        public string Callsign;

        [JsonProperty("websiteurl")]
        public string Websiteurl;

        [JsonProperty("station_id")]
        public string StationId;

       
    }

    public class RootStream
    {
        [JsonProperty("success")]
        public bool Success;

        [JsonProperty("result")]
        public List<ResultStream> Result;
    }

}
