using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
    [Table("page_block")]
    public class PageBlock : BaseEntity
    {
        [Column("eo_type")]
        public eEoTypes EoType { get; set; } = eEoTypes.PAGE_BLOCK;
        [Column("eo_subtype")]
        public eEoSubTypes EoSubtype { get; set; } = eEoSubTypes.INSTANCE;
        [Column("parent_id")]
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public string Name4 { get; set; }
        public string Name5 { get; set; }
        public string Name6 { get; set; }
        public string Name7 { get; set; }
        public string Name8 { get; set; }
        public string Url1 { get; set; }
        public string Url2 { get; set; }
        public string Url3 { get; set; }
        public string Url4 { get; set; }
        public string Url5 { get; set; }
        public string Url6 { get; set; }
        public string Url7 { get; set; }
        public string Url8 { get; set; }
    }

}
