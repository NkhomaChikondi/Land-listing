using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Land_listing.Models
{
    public class Notification
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        [Indexed]
        public int userId { get; set; }
    }
}
