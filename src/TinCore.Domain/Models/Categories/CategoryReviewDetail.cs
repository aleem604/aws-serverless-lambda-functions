using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
    [Table("category_review_detail")]
  public  class CategoryReviewDetail: BaseEntity
    {
        public eEoTypes EoType { get; set; } = eEoTypes.REVIEW_CATEGORY_LI;
        public eEoSubTypes EoSubtype { get; set; } = eEoSubTypes.INSTANCE;
        public string Name { get; set; }
        [Column("full_name")]
        public string FullName { get; set; }
        public string Description { get; set; }
        [Column("category_review_id")]
        public long CategoryReviewId { get; set; }
        public virtual CategoryReview CategoryReview { get; set; }
    }
}
