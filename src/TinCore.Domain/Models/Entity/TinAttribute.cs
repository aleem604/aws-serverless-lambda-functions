using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
    [Table("attribute")]
    public class TinAttribute : BaseEntity
    {
        [Column("eo_type")]
        public eEoTypes EoType { get; set; } = eEoTypes.ENTITY;
        [Column("eo_subtype")]
        public eEoSubTypes EoSubtype { get; set; } = eEoSubTypes.INSTANCE;
        public string Name { get; set; }
        [Column("seq_no")]
        public int SeqNo { get; set; }
    }

}
