-- =============================================
-- Create View template
-- =============================================
USE ProManDB
GO

IF object_id(N'dbo.v_profile', 'V') IS NOT NULL
	DROP VIEW dbo.v_profile
GO

CREATE VIEW dbo.v_profile AS
select  id, entity_id, profile_type, profile_subtype, eo_type, eo_subtype, rating, null as reviewed_by, null as stars, null as review_date, desc1, desc2, image_url, lat, lng, price, null as comment_text, null as photo_urls from profile_attribute
union
select   id, entity_id, profile_type, profile_subtype, eo_type, eo_subtype, null as rating, reviewed_by, stars, review_date,desc1, desc2, image_url, null as lat, null as lng, null as price, comment_text, photo_urls  from profile_review
union
select id, entity1_id, entity1_type, entity1_subtype,entity2_id, entity2_type, entity2_subtype, relation_type, relation_subtype, entity1_name, entity2_name  ,status from location_entity_relation
union
select id, entity1_id, entity1_type, entity1_subtype,entity2_id, entity2_type, entity2_subtype, relation_type, relation_subtype, entity1_name, entity2_name  ,status from location_category_relation

-- select * from v_profile