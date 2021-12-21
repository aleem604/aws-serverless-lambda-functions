using System;
using System.Collections.Generic;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Application.ViewModels
{
    public class TrackingViewModel: BaseViewModel
    {
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long EntitySubscriptionId { get; set; }
        public long EntityId { get; set; }
    }
}
