using System;
using System.Collections.Generic;
using System.Text;

namespace MOB_RadioApp.Models
{
    public class User
    {
        public string EmailAddress { get; set;  }
        public string Password { get; set; }   
        public List<int> FavoriteStations { get; set; }
        public Dictionary<int,int> HistoryStations { get; set; }
        
    }
}
