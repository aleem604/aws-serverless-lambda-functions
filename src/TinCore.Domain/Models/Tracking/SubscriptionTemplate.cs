using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
    [Table("subscription_template")]
   public class SubscriptionTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public eRecordStatus Status { get; set; }
        [Column("status_change_date")]
        public DateTime? StatusChangeDate { get; set; }
        [Column("status_changed_by")]
        public long? StatusChangedBy { get; set; }
        [Column("create_date")]
        public DateTime CreateDate { get; set; }
        [Column("created_by")]
        public long CreatedBy { get; set; }
        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }
        [Column("updated_by")]
        public long UpdatedBy { get; set; }

    }
}
