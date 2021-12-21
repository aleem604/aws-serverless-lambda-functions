using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
    [Table("category_review")]
   public class CategoryReview: BaseEntity
    {
        public eEoTypes EoType { get; set; } = eEoTypes.REVIEW_CATEGORY_UL;
        public eEoSubTypes EoSubtype { get; set; } = eEoSubTypes.INSTANCE;
        public string Name { get; set; }
        [Column("full_name")]
        public string FullName { get; set; }
        [Column("classification_id")]
        public long ClassificationId { get; set; }
        public virtual Classification classification { get; set; }
        public virtual IEnumerable<CategoryReviewDetail> CategoryReviewDetail { get; set; }
    }
}
