using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Land_listing.Models
{
    public class Land
    {
        [PrimaryKey, AutoIncrement]
        public int LandId { get; set; }
        public string LandName { get; set; }
        public string LandDescription { get; set;}
        public string Location { get; set; }
        public string PlotSize { get; set; }
        public double Price { get; set; }
    }
}
