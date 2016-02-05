using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemoryAPI.Models
{
    public class Media
    {
        public int id { get; set; }
        public string fileUrl { get; set; }
        public string container { get; set; }
        public User user { get; set; }
    }
}