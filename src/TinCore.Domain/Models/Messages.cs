using System;
using System.Collections.Generic;
using System.Text;

namespace TinCore.Domain.Models
{
    public class Message
    {
        public int isn { get; set; }
        public string message_group { get; set; }
        public string message_code { get; set; }
        public int message_ncode { get; set; }
        public string language { get; set; }
        public string display_value { get; set; }
        public string long_comment { get; set; }
    }
}
