using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemoryAPI.Models
{
    public class SoundMedia : Media
    {
        public float duration { get; set; }
        public string codec { get; set; }
        public float bitRate { get; set; }
        public int channels { get; set; }
        public float samplingRate { get; set; }
    }
}