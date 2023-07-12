using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Land_listing.Models
{
    public class User_Land
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int UserId { get; set; }
        public int landId { get; set; }
    }
}
