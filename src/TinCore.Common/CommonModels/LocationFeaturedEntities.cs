using System;
using System.Collections.Generic;
using System.Text;

namespace TinCore.Common.CommonModels
{
  public  class LocationFeaturedEntities
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal? rating { get; set; }
        public IEnumerable<LocationFeaturedCategories> categories { get; set; } = new List<LocationFeaturedCategories>();
    }
    public class LocationFeaturedCategories
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
    }
}
