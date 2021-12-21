using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
    [Table("entity_subscription_tracking")]
    public class EntitySubscriptionTracking : BaseEntity
    {
        [Column("eo_type")]
        public eEoTypes EoType { get; set; } = eEoTypes.ENTITY;
        [Column("eo_subtype")]
        public eEoSubTypes EoSubtype { get; set; } = eEoSubTypes.INSTANCE;
        [Column("tracking_type")]
        public eTrackingType TrackingType { get; set; }
        [Column("tracking_subtype")]
        public eEoSubTypes TrackingSubtype { get; set; } = eEoSubTypes.INSTANCE;
        [Column("start_date")]
        public DateTime? StartDate { get; set; }
        [Column("end_date")]
        public DateTime? EndDate { get; set; }
        public string Name { get; set; }
        [Column("entity_id")]
        public long EntityId { get; set; }
        public virtual TinEntity Entity { get; set; }
        [Column("subscription_id")]
        public long SubscriptionId { get; set; }
        public virtual Subscription Subscription { get; set; }


    }

}
