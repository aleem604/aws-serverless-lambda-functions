using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Domain.Models
{
    [Table("location_entity_relation")]
    public class LocationEntityRelation : BaseEntity
    {
        [Column("entity1_id")]
        public long Entity1Id { get; set; }
        [Column("entity1_type")]
        public eEoTypes Entity1Type { get; set; } = eEoTypes.ENTITY;
        [Column("entity1_subtype")]
        public eEoSubTypes Entity1Subtype { get; set; } = eEoSubTypes.INSTANCE;
        [Column("entity2_id")]
        public long Entity2Id { get; set; }
        [Column("entity2_type")]
        public eEoTypes Entity2Type { get; set; } = eEoTypes.ENTITY;
        [Column("entity2_subtype")]
        public eEoSubTypes Entity2Subtype { get; set; } = eEoSubTypes.INSTANCE;
        [Column("relation_type")]
        public eRelationTypes RelationType { get; set; } = eRelationTypes.LOCATION_EO;
        [Column("relation_subtype")]
        public eEoSubTypes RelationSubtype { get; set; }
        [Column("entity1_name")]
        public string Entity1Name { get; set; }
        [Column("entity2_name")]
        public string Entity2Name { get; set; }
        public string Comment { get; set; }
    }
}
