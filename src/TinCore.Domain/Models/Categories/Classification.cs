using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
    public class Classification: BaseEntity
    {
        [Column("eo_type")]
        public eEoTypes EoType { get; set; } = eEoTypes.CLASSIFICATION;
        [Column("eo_subtype")]
        public eEoSubTypes EoSubtype { get; set; } = eEoSubTypes.INSTANCE;
        [Column("parent_id")]
        public long ParentId { get; set; }
        public string Name { get; set; }
        [Column("full_name")]
        public string FullName { get; set; }       
        public virtual ICollection<ClassificationType> ClassificationType { get; set; }
    }
}