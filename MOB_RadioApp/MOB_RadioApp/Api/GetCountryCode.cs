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
using XamarinCountryPicker.Models;

namespace MOB_RadioApp.Services
{
    public class GetCountryCode
    { 
        public async Task<ObservableCollection<CountryModel>> GetCountriesAsync()
        {
            OpenRadioInfo<ObservableCollection<CountryModel>> _openRadioInfo = new OpenRadioInfo<ObservableCollection<CountryModel>>();  
            string type = "countrycodes/";
            var countries = await _openRadioInfo.ApiCall(type);
            return countries;
        }

    }
}
