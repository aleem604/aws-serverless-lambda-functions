using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Application.ViewModels
{
  public  class BaseViewModel
    {
        public long Id { get; set; }
        public eRecordStatus Status { get; set; }
        //public DateTime? StatuChangeDate { get; set; }
        //public long? StatusChangedBy { get; set; }
        //public DateTime CreateDate { get; set; }
        //public long CreatedBy { get; set; }
        //public DateTime? UpdateDate { get; set; }
        //public long? UpdatedBy { get; set; }
    }
}
