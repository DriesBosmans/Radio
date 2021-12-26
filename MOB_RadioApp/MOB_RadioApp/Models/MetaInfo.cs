using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOB_RadioApp.Models
{
    public class MetaInfo
    {
        public class MetaData
        {
            [JsonProperty("callsign")]
            public string Callsign { get; set; }

            [JsonProperty("genre")]
            public string Genre { get; set; }

            [JsonProperty("band")]
            public string Band { get; set; }

            [JsonProperty("artist")]
            public string Artist { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("songstamp")]
            public string Songstamp { get; set; }

            [JsonProperty("seconds_remaining")]
            public int SecondsRemaining { get; set; }

            [JsonProperty("station_id")]
            public string StationId { get; set; }
        }

        public class RootMeta
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("result")]
            public List<MetaData> MetaDatas { get; set; }
        }


    }
}
