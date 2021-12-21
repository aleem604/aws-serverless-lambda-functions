using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
    public class BaseLocation : BaseEntity
    {
        [Column("location_type")]
        public eLocationType LocationType { get; set; }
        public string Name { get; set; }
        [Column("full_name")]
        public string FullName { get; set; }
        [Column("short_name")]
        public string ShortName { get; set; }
        public string Code { get; set; }
        public string Desc1 { get; set; }
        public string Desc2 { get; set; }
        public string Desc3 { get; set; }
        [Column("image_url")]
        public string ImageUrl { get; set; }
        public string Featured { get; set; }
        public string Admin { get; set; }
        [Column("country_meta")]
        public string CountryMeta { get; set; }
        [Column("region_meta")]
        public string RegionMeta { get; set; }
        [Column("city_meta")]
        public string CityMeta { get; set; }
        [Column("neighbourhood_meta")]
        public string NeighbourhoodMeta { get; set; }
        [Column("site_owner")]
        public string SiteOwner { get; set; }
        [Column("donotmiss_entities")]
        public string DonotmissEntities { get; set; }
        [Column("featured_entities")]
        public string FeaturedEntities { get; set; }

    }
}
