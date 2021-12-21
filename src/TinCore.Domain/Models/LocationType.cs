using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
    [Table("location_type")]
    public class LocationType : BaseEntity
    {
        [Column("eo_type")]
        public eEoTypes EoType { get; set; } = eEoTypes.LOCATIONTYPE;
        [Column("eo_subtype")]
        public eEoSubTypes EoSubtype { get; set; } = eEoSubTypes.INSTANCE;
        [Column("parent_id")]
        public int ParentId { get; set; }
        [Column("location_type_id")]
        public int LocationTypeId { get; set; }
        public string Name { get; set; }
        [Column("full_name")]
        public string FullName { get; set; }
        [Column("short_name")]
        public string ShortName { get; set; }
        public string Code { get; set; }
        public string Desc1 { get; set; }
        public string Desc2 { get; set; }
        public string Desc3 { get; set; }
        public string Image_url { get; set; }
        [Column("location_featured_ids")]
        public string LocationFeatureIds { get; set; }
        public string Admin { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Neighbourhood { get; set; }
        public string Region { get; set; }
        [Column("site_owner")]
        public string Site_Owner { get; set; }
        [Column("donotmiss_entities")]
        public string DonotmissEntities { get; set; }       
    }

}
