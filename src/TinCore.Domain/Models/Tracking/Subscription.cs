using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
    [Table("subscription")]
    public class Subscription : BaseEntity
    {
        [Column("eo_type")]
        public eEoTypes EoType { get; set; } = eEoTypes.SUBSCRIPTION;
        [Column("eo_subtype")]
        public eEoSubTypes EoSubtype { get; set; } = eEoSubTypes.INSTANCE;       
        public string Name { get; set; }
        [Column("tracking_template_id")]
        public long TrackingTemplateId { get; set; }
        public virtual SubscriptionTemplate TrackingTemplate { get; set; }
    }

}
