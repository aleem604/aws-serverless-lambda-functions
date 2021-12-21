using System;
using System.Collections.Generic;
using System.Text;

namespace TinCore.Domain.Models
{
    public class Country : BaseLocation
    {
        public virtual ICollection<Region> Region { get; set; }
    }
}
