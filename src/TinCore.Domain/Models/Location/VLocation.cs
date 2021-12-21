using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
    [Table("v_location")]
    public class VLocation
    {
        public long Id { get; set; }
        [Column("parent_id")]
        public long? ParentId { get; set; }
        public string Name { get; set; }
        [Column("full_name")]
        public string FullName { get; set; }
        [Column("short_name")]
        public string ShortName { get; set; }
        public string Code { get; set; }
        [Column("location_type")]
        public eLocationType LocationType { get; set; }
        public string Desc1 { get; set; }
        public string Desc2 { get; set; }
        [Column("image_url")]
        public string ImageUrl { get; set; }       
        public string Featured { get; set; }
        [Column("featured_entities")]
        public string FeaturedEntities { get; set; }
        public eRecordStatus Status { get; set; }
    }
}
