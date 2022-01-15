using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOB_RadioApp.Models
{
    public class SqlStation
    {
        [PrimaryKey]
        public string Id { get; set; }

        public SqlStation(string id)
        {
            Id = id; 
        }
        public SqlStation()
        {

        }
    }
}
