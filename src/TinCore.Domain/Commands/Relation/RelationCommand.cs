using System;
using TinCore.Common.Enums;
using TinCore.Domain.Core.Commands;
using TinCore.Domain.Models;

namespace TinCore.Domain.Commands
{
    public abstract class RelationCommand : Command
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public eRecordStatus Status { get; set; }
        public string MetaData { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedBy { get; set; }
        public string VideoUrl { get; set; }
        public long Location_Id { get; set; }
        public long Category_Id { get; set; }
        public string City_Featured { get; set; }
        public string Region_Featured { get; set; }
        public string City_Donotmiss { get; set; }
        public string Region_Donotmiss { get; set; }
        public EntityContact Contact { get; set; }
        public ProfileAttribute ProfileAttribute { get; set; }
        public ProfileReview ProfileReview { get; set; }
        public ProfileSection ProfileSection { get; set; }
        public EntitySubscriptionTracking EntitySubscriptionTracking { get; set; }

    }
}