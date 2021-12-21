using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
    [Table("entity")]
    public class TinEntity : BaseEntity
    {
        [Column("eo_type")]
        public eEoTypes EoType { get; set; } = eEoTypes.ENTITY;
        [Column("eo_subtype")]
        public eEoSubTypes EoSubtype { get; set; } = eEoSubTypes.INSTANCE;
        public string Name { get; set; }
        [Column("short_name")]
        public string ShortName { get; set; }
        [Column("full_name")]
        public string FullName { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public string Desc1 { get; set; }
        public string Desc2 { get; set; }
        public string Desc3 { get; set; }
        [Column("video_url")]
        public string VideoUrl { get; set; }
        public EntityContact Contact { get; set; }
        public ProfileAttribute ProfileAttribute { get; set; }
        public ProfileReview ProfileReview { get; set; }
        public ProfileSection ProfileSection { get; set; }        
    }

}
