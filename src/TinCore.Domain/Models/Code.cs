using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TinCore.Domain.Models
{
    [Table("Codes")]
    public class Code
    {
        [Key]
        public int isn { get; set; }
        public string code_group { get; set; }
        public string code_subgroup { get; set; }
        public string code_type { get; set; }
        public int display_order { get; set; }
        public decimal code_value_num { get; set; }
        public string code_value_str { get; set; }
        public string notes { get; set; }
    }

}
