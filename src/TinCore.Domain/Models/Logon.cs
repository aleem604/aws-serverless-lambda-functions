using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TinCore.Domain.Models
{
    public class Logon
    {
        [Key]
        public int isn { get; set; }
        public int eo_isn { get; set; }
        public string logon_type { get; set; }
        public string logon1 { get; set; }
        public string logon2 { get; set; }
        public string pwd { get; set; }
        public string recovery_question { get; set; }
        public string recovery_answer { get; set; }
        public string notes { get; set; }
        public short status { get; set; }
        public DateTime status_timestamp { get; set; }
        public int status_changed_by_user { get; set; }
        public DateTime create_timestamp { get; set; }
        public int created_by_user { get; set; }
        public DateTime last_update_timestamp { get; set; }
        public int last_updated_by_user { get; set; }
    }

}
