using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
    [Table("profile_attribute")]
    public class ProfileAttribute
    {
        public long Id { get; set; }
        [Column("profile_type")]
        public eProfileType ProfileType { get; set; } = eProfileType.Attribute;
        [Column("profile_subtype")]
        public eProfileSubType ProfileSubtype { get; set; } = eProfileSubType.Profile1;
        [Column("eo_type")]
        public eEoTypes EoType { get; set; } = eEoTypes.ENTITY;
        [Column("eo_subtype")]
        public eEoSubTypes EoSubtype { get; set; } = eEoSubTypes.INSTANCE;
        public short Rating { get; set; }
        public string Desc1 { get; set; }
        public string Desc2 { get; set; }
        [Column("image_url")]
        public string ImageUrl { get; set; }
        [Column("lat")]
        public string Latitude { get; set; }
        [Column("lng")]
        public string Longitude { get; set; }
        public string Price { get; set; }
        [Column("entity_id")]
        public long EntityId { get; set; }
        public virtual TinEntity Entity { get; set; }
    }
}
