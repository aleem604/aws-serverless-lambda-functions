
-- =========================================
-- Create EntityProfile table template
--EO_TYPE~ENTITY  1010
--
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.profile_attribute', 'U') IS NOT NULL
  DROP TABLE dbo.profile_attribute
GO

CREATE TABLE [dbo].[profile_attribute](
	[id] [bigint] IDENTITY(1000001,1) NOT NULL,
	[entity_id][bigint] NOT NULL,
	[profile_type][smallint] NOT NULL,
	[profile_subtype][smallint] NOT NULL,
	[eo_type][smallint] NOT NULL,
    [eo_subtype][smallint] NOT NULL,
	[rating][smallint] NULL,
	[desc1] [varchar](7000) NULL,
	[desc2] [varchar](7000) NULL,
	[image_url] [varchar](500) NULL,
	[lat] [varchar](250) NULL,
	[lng] [varchar](250) NULL,	
	[price] [varchar](250) NULL,
 CONSTRAINT [PK_profile_attribute] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------

SET IDENTITY_INSERT dbo.profile_attribute ON

-- PROFILE~ATTRIBUTES' => 8020
insert into [dbo].[profile_attribute] (id, entity_id, profile_type,profile_subtype, eo_type, eo_subtype, rating, desc1,desc2, image_url, lat, lng,price)
					select isn, eo_isn, profile_type,profile_subtype, eo_type, eo_subtype, profile1, 
					IIF(profile_subtype =1, CONVERT(xml, profile9).value('(/row/@desc1)[1]', 'varchar(4000)'),''), 
					IIF(profile_subtype =1, CONVERT(xml, profile9).value('(/row/@desc2)[1]', 'varchar(4000)'),''), 
					IIF(profile_subtype=1, profile6,''),  -- image_url
					IIF(profile_subtype=2, profile6,''),  -- lat
					IIF(profile_subtype=2, profile7,''),  -- lng
					IIF(CHARINDEX('$', profile7)>0, profile7,'')  -- price
from profiles where profile_type in (8020)

SET IDENTITY_INSERT dbo.profile_attribute OFF

--  select * from profile_attribute
