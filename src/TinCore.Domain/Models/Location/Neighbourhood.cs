using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TinCore.Domain.Models
{
    public class Neighbourhood : BaseLocation
    {
        [Column("city_id")]
        public long CityId { get; set; }
    }
}
