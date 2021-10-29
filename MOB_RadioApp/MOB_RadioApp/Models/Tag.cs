using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MOB_RadioApp.Models
{
    public static class AllTags
    {
        public static ObservableCollection<Tag> LstTags { get; set; } = new ObservableCollection<Tag>();
    }
    public class Tag
    {
        public string Name { get; set; }
        public int Stationcount { get; set; }
    }
}
