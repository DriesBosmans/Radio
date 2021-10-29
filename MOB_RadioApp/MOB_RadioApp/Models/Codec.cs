using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MOB_RadioApp.Models
{
    public static class AllCodecs
    {
        public static ObservableCollection<Codec> LstCodecs { get; set; } = new ObservableCollection<Codec>();
    }
    public class Codec
    {
        public string Name { get; set; }
        public int Stationcount { get; set; }
    }
}
