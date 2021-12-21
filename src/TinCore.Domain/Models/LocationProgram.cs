using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
    [Table("location_program")]
    public class LocationProgram : BaseEntity
    {
        public eEoTypes EoType { get; set; } = eEoTypes.LOCATION_PROGRAM;
        [Column("eo_subtype")]
        public eEoSubTypes EoSubtype { get; set; } = eEoSubTypes.INSTANCE;
        [Column("location_id")]
        public int LocationId { get; set; }
        public string Name { get; set; }       
    }
}
