using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemoryAPI.Models
{
    public class User
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        //public List<Vacation> vacationList { get; set; }
        public List<User> friendList { get; set; }
        public List<Media> mediaList { get; set; }

    }
}