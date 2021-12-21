-- =============================================
-- Create View template
-- =============================================
USE ProManDB
GO

IF object_id(N'dbo.v_relation', 'V') IS NOT NULL
	DROP VIEW dbo.v_relation
GO

CREATE VIEW dbo.v_relation AS
select id, entity1_id, entity1_type, entity1_subtype,entity2_id, entity2_type, entity2_subtype, relation_type, relation_subtype, entity1_name, entity2_name  ,status from relation_entity_relation
union
select id, entity1_id, entity1_type, entity1_subtype,entity2_id, entity2_type, entity2_subtype, relation_type, relation_subtype, entity1_name, entity2_name  ,status from entity_attribute_relation
union
select id, entity1_id, entity1_type, entity1_subtype,entity2_id, entity2_type, entity2_subtype, relation_type, relation_subtype, entity1_name, entity2_name  ,status from location_entity_relation
union
select id, entity1_id, entity1_type, entity1_subtype,entity2_id, entity2_type, entity2_subtype, relation_type, relation_subtype, entity1_name, entity2_name  ,status from location_category_relation

-- select * from v_relation