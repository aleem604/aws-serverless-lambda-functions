using System;
using System.Collections.Generic;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Application.ViewModels
{
    public class ProfileAttributeViewModel
    {
        public long Id { get; set; }
        public string Desc1 { get; set; }
        public string Desc2 { get; set; }
        public string ImageUrl { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Price { get; set; }
        public long EntityId { get; set; }
    }
    public class ProfileReviewViewModel
    {
        public long Id { get; set; }
        public long? ReviewedBy { get; set; }
        public decimal? Stars { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string Desc1 { get; set; }
        public string Desc2 { get; set; }
        public string ImageUrl { get; set; }
        public string CommentText { get; set; }
        public string PhotoUrls { get; set; }
        public long EntityId { get; set; }        
    }

    public class ProfileSectionViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string CommentText { get; set; }
        public string PhotoUrls { get; set; }
        public long EntityId { get; set; }
    }
}
