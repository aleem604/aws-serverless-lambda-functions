using System.ComponentModel.DataAnnotations.Schema;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
    [Table("classification_type", Schema ="dbo")]
    public class ClassificationType: BaseEntity
    {
        [Column("eo_type")]
        public eEoTypes EoType { get; set; } = eEoTypes.CLASSIFICATIONTYPE;
        [Column("eo_subtype")]
        public eEoSubTypes EoSubtype { get; set; } = eEoSubTypes.INSTANCE;
        public string Name { get; set; }
        [Column("full_name")]
        public string FullName { get; set; }
        [Column("image_url")]
        public string ImageUrl { get; set; }
        public string Desc1 { get; set; }
        [Column("classification_id")]
        public long ClassificationId { get; set; }
        public virtual Classification Classification { get; set; }
    }
}