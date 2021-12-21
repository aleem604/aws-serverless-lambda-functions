
-- =========================================
-- Create EntityProfile Section table template
-- PROFILE~SECTION' => 8021
--
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.profile_section', 'U') IS NOT NULL
  DROP TABLE dbo.profile_section
GO

CREATE TABLE [dbo].[profile_section](
	[id] [bigint] IDENTITY(1000001,1) NOT NULL,
	[entity_id][bigint] NOT NULL,
	[profile_type][smallint] NOT NULL,
	[profile_subtype][smallint] NOT NULL,
	[eo_type][smallint] NOT NULL,
    [eo_subtype][smallint] NOT NULL,
	[name] [varchar](250) NULL,
	[image_url] [varchar](500) NULL,
	[description] [varchar](7000) NULL,
	[comment_text] [varchar](500) NULL,
	[photo_urls] [varchar](500) NULL,
 CONSTRAINT [PK_profile_section] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------

SET IDENTITY_INSERT dbo.profile_section ON

-- PROFILE~SECTION' => 8021
insert into [dbo].[profile_section] (id, entity_id, profile_type, profile_subtype, eo_type, eo_subtype, name, image_url, description, comment_text, photo_urls)
					select isn, eo_isn, profile_type, profile_subtype, eo_type, eo_subtype, profile6, profile7, 					
					IIF(CHARINDEX('row', profile9)>0, CONVERT(xml, profile9).value('(/row/@desc2)[1]', 'varchar(4000)'), profile9) as desc2,
					IIF(CHARINDEX('row', profile9)>0, CONVERT(xml, profile9).value('(/row/@comment_text)[1]', 'varchar(4000)'), '') as comment_text,
					IIF(CHARINDEX('row', profile9)>0, CONVERT(xml, profile9).value('(/row/@photo_urls)[1]', 'varchar(4000)'), '') as photo_urls
from profiles where profile_type in (8021)

SET IDENTITY_INSERT dbo.profile_section OFF

--	select * from profile_section order by 1 desc
