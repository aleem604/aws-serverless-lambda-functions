-- =============================================
-- Create View template
-- =============================================
USE ProManDB
GO

IF object_id(N'dbo.v_location', 'V') IS NOT NULL
	DROP VIEW dbo.v_location
GO

CREATE VIEW dbo.v_location AS
select id, null as parent_id, name,full_name, short_name, code, location_type, desc1, desc2, image_url, featured, featured_entities, status from country
union
select id, country_id, name,full_name, short_name, code, location_type, desc1, desc2, image_url, featured, featured_entities, status from region
union
select id, region_id, name,full_name, short_name, code, location_type, desc1, desc2, image_url, featured, featured_entities, status from city
union
select id, city_id, name,full_name, short_name, code, location_type, desc1, desc2, image_url, featured, featured_entities, status from neighbourhood


-- select * from country