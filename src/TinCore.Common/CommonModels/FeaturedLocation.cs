using System;
using System.Collections.Generic;
using System.Text;

namespace TinCore.Common.CommonModels
{
    public class Featured
    {
        public int id { get; set; }
        public string desc1 { get; set; }
        public string desc2 { get; set; }
        public string image_url { get; set; }
    }

    public class FeaturedLocation
    {
        public int id { get; set; }
        public string name { get; set; }
        public string desc1 { get; set; }
        public string desc2 { get; set; }
        public string image_url { get; set; }
        public List<Featured> featured { get; set; } = new List<Featured>();
    }
}
