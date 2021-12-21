using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TinCore.Domain.Models
{
    [Table("Contact")]
    public class TinContact
    {
        [Key]
        public int isn { get; set; }
        public int eo_isn { get; set; }
        public short eo_type { get; set; }
        public short eo_subtype { get; set; }
        public short contact_type { get; set; }
        public short contact_subtype { get; set; }
        public int hisn1 { get; set; }
        public int hisn2 { get; set; }
        public int hisn3 { get; set; }
        public int hisn4 { get; set; }
        public int hisn5 { get; set; }
        public string phone { get; set; }
        public string cell { get; set; }
        public string fax { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string country { get; set; }
        public string contact1 { get; set; }
        public string contact2 { get; set; }
        public string contact3 { get; set; }
        public string contact4 { get; set; }
        public string contact5 { get; set; }
        public string contact6 { get; set; }
        public string contact7 { get; set; }
        public string contact8 { get; set; }
        public string contact9 { get; set; }
        public short status { get; set; }
        public DateTime? status_timestamp { get; set; }
        public int status_changed_by_user { get; set; }
        public DateTime create_timestamp { get; set; }
        public int created_by_user { get; set; }
        public DateTime? last_update_timestamp { get; set; }
        public int last_updated_by_user { get; set; }
        public string comment { get; set; }
        public string long_comment { get; set; }
    }
}
