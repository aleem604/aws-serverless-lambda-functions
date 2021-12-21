using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
    [Table("profile_review")]
    public class ProfileReview
    {
        public long Id { get; set; }
        [Column("profile_type")]
        public eProfileType ProfileType { get; set; } = eProfileType.Review;
        [Column("profile_subtype")]
        public eProfileSubType ProfileSubtype { get; set; } = eProfileSubType.Profile1;
        [Column("eo_type")]
        public eEoTypes EoType { get; set; } = eEoTypes.ENTITY;
        [Column("eo_subtype")]
        public eEoSubTypes EoSubtype { get; set; } = eEoSubTypes.INSTANCE;
        [Column("Reviewed_by")]
        public long? ReviewedBy { get; set; }
        public decimal? Stars { get; set; }
        [Column("review_date")]
        public DateTime? ReviewDate { get; set; }
        public string Desc1 { get; set; }
        public string Desc2 { get; set; }
        [Column("image_url")]
        public string ImageUrl { get; set; }
        [Column("comment_text")]
        public string CommentText { get; set; }
        [Column("photo_urls")]
        public string PhotoUrls { get; set; }
        [Column("entity_id")]
        public long EntityId { get; set; }
        public virtual TinEntity Entity { get; set; }
    }
}
