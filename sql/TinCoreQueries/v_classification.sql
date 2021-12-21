-- =============================================
-- Create View template
-- =============================================
USE ProManDB
GO

IF object_id(N'dbo.v_classification', 'V') IS NOT NULL
	DROP VIEW dbo.v_classification
GO

CREATE VIEW dbo.v_classification AS
select id, eo_type, eo_subtype, parent_id, null as classification_id, name, full_name, null as image_url, status from classification
union
select id, eo_type, eo_subtype,null as parent_id, classification_id, name, full_name, image_url, status  from classification_type


-- select * from v_classification