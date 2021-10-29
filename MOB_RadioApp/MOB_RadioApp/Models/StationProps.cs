using System;
using System.Collections.Generic;
using System.Text;

namespace MOB_RadioApp.Models
{
    public abstract class StationProps
    {
        public bool IsFavorite { get; set; } = false;
        public int Order { get; set; } = 4;
    }
}
