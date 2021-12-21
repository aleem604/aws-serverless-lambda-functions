using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TinCore.Domain.Models
{
    public class Session
    {
        [Key]
        public string session_id { get; set; }
        public DateTime create_timestamp { get; set; }
        public DateTime last_update_timestamp { get; set; }
        public string browser_version { get; set; }
        public int user_isn { get; set; }
        public int menu_isn { get; set; }
        public int role_isn { get; set; }
        public string long_comment { get; set; }
    }
}
