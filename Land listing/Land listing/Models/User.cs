using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Land_listing.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int UserId { get; set; }
        public string FullName { get; set; }  
        public string Username { get; set; }  
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Usertype { get; set; }   
        public DateTime CreatedOn { get; set; }


    }
}
