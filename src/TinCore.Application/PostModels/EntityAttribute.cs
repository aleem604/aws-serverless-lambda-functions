using System;
using System.Collections.Generic;
using System.Text;

namespace TinCore.Application
{
    public class EntityAttribute
    {
        public string api_key { get; set; }
        public long id { get; set; }
        public long eo_id { get; set; }
        public int attribute_id { get; set; }
        public int status { get; set; }
    }
}
