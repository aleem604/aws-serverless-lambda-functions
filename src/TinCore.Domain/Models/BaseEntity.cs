using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
  public  class BaseEntity
    {
        public long Id { get; set; }
        public eRecordStatus Status { get; set; }
        [Column("status_change_date")]
        public DateTime? StatuChangeDate { get; set; }
        [Column("Status_changed_by")]
        public long? StatusChangedBy { get; set; }
        [Column("create_date")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Column("created_by")]
        public long CreatedBy { get; set; } = 0;
        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }
        [Column("updated_by")]
        public long? UpdatedBy { get; set; }
    }
}
