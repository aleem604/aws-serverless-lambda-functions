using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TinCore.Domain.Models
{
    public class Region : BaseLocation
    {
        [Column("country_id")]
        public long CountryId { get; set; }
    }
}
