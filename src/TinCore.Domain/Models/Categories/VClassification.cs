using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
    [Table("v_classification")]
    public class VClassification
    {
        public long Id { get; set; }
        [Column("classification_id")]
        public long? ClassificationId { get; set; }
        public eEoTypes EoType { get; set; }
        public eEoSubTypes EoSubtype { get; set; }
        [Column("parent_id")]
        public long ParentId { get; set; }
        public string Name { get; set; }
        [Column("full_name")]
        public string FullName { get; set; }
        [Column("image_url")]
        public string ImageUrl { get; set; }

        public eRecordStatus Status { get; set; }

    }
}