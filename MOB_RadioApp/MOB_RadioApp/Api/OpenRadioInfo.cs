using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MOB_RadioApp.Api
{
    public class OpenRadioInfo<T>
    {
        HttpClient _httpClient = new HttpClient();
        private const string baseUrl = "https://nl1.api.radio-browser.info/json/";
      
        public async Task<T> ApiCall(string type, string country="", string limiturl = "", string hideBroken = "", string orderBy = "")
        {
            T DeserializedObject = default(T);
            string url = baseUrl + type + country + limiturl + hideBroken + orderBy;
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                DeserializedObject = JsonConvert.DeserializeObject<T>(body);
            }
            return DeserializedObject;
        }
    }
}
