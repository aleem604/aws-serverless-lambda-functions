using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TinCore.Domain.Models
{
    public class City : BaseLocation
    {
        [Column("region_id")]
        public long RegionId { get; set; }
    }
}
