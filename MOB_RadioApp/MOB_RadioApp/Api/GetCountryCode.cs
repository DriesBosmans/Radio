using MOB_RadioApp.Api;
using MOB_RadioApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MOB_RadioApp.Services
{
    public class GetCountryCode
    { 
        public async Task<ObservableCollection<Country>> GetCountriesAsync()
        {
            OpenRadioInfo<ObservableCollection<Country>> _openRadioInfo = new OpenRadioInfo<ObservableCollection<Country>>();  
            string type = "countrycodes/";
            var countries = await _openRadioInfo.ApiCall(type);
            return countries;
        }

    }
}
