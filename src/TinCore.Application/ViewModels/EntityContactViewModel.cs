using System;
using System.Collections.Generic;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Application.ViewModels
{
   public class BusinessContactViewModel
    {
        public long Id { get; set; }
        public long EntityId { get; set; }
        public eContactType ContactType { get; set; } = eContactType.Business;
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public DateTime CreateDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
