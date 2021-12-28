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
    public class ApiService
    {
       
        //public async Task<RangedObservableCollection<OldStation>> GetStationsAsync2(string country, int offset = 0)
        //{
        //    OpenRadioInfo<RangedObservableCollection<OldStation>> _openRadioInfo = new OpenRadioInfo<RangedObservableCollection<OldStation>>();
        //    string type = "stations/bycountrycodeexact/";
        //    string limiturl = $"?limit={ProjectSettings.Limit}&offset={offset}";
        //    const string hideBroken = "&hidebroken=true";
        //    string orderBy = $"&order=votes&reverse=true";
        //    var stations = await _openRadioInfo.ApiCall(type, country, limiturl, hideBroken, orderBy);
        //    return stations;
        //}

        public async Task<ObservableCollection<Country>> GetCountriesAsync()
        {
            OpenRadioInfo<ObservableCollection<Country>> _openRadioInfo = new OpenRadioInfo<ObservableCollection<Country>>();  
            string type = "countrycodes/";
            var countries = await _openRadioInfo.ApiCall(type);
            return countries;
        }

        //public async Task<ObservableCollection<Tag>> GetTagsAsync()
        //{
        //    OpenRadioInfo<ObservableCollection<Tag>> _openRadioInfo = new OpenRadioInfo<ObservableCollection<Tag>>();
        //    string type = "tags";
        //    var tags = await _openRadioInfo.ApiCall(type);
        //    return tags;
        //}

        //public async Task<ObservableCollection<Codec>> GetCodecsAsync()
        //{
        //    OpenRadioInfo<ObservableCollection<Codec>> _openRadioInfo = new OpenRadioInfo<ObservableCollection<Codec>>();
        //    string type = "codecs";
        //    var codecs = await _openRadioInfo.ApiCall(type);
        //    return codecs;
        //}

    }
}
