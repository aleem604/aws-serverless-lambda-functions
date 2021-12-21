using System;
using System.Collections.Generic;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Application.ViewModels
{
    public class EntityViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public string Desc1 { get; set; }
        public string Desc2 { get; set; }
        public string Desc3 { get; set; }
        public string VideoUrl { get; set; }
        public long Location_Id { get; set; }
        public long Category_Id { get; set; }
        public string City_Featured { get; set; }
        public string Region_Featured { get; set; }
        public string City_Donotmiss { get; set; }
        public string Region_Donotmiss { get; set; }
        public BusinessContactViewModel Contact { get; set; }
        public ProfileAttributeViewModel ProfileAttribute { get; set; }
        public ProfileReviewViewModel ProfileReview { get; set; }
        public ProfileSectionViewModel ProfileSection { get; set; }
        public TrackingViewModel Tracking { get; set; }
    }
}
