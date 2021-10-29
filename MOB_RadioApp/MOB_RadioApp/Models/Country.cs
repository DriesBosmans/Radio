using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MOB_RadioApp.Models
{
    public static class AllCountries
    {
        public static ObservableCollection<Country> LstCountries { get; set; } = new ObservableCollection<Country>();
    }
    public class Country
    {
        public string Name { get; set; }
        public int Stationcount { get; set; }
    }
}
