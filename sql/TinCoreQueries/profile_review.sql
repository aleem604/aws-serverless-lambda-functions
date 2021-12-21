
-- =========================================
-- Create EntityProfile Review table template
--PROFILE~REVIEW' => 8020
--
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.profile_review', 'U') IS NOT NULL
  DROP TABLE dbo.profile_review
GO

CREATE TABLE [dbo].[profile_review](
	[id] [bigint] IDENTITY(1000001,1) NOT NULL,
	[entity_id][bigint] NOT NULL,
	[profile_type][smallint] NOT NULL,
	[profile_subtype][smallint] NOT NULL,
	[eo_type][smallint] NOT NULL,
    [eo_subtype][smallint] NOT NULL,
	[reviewed_by][bigint] NULL,
	[stars] [decimal](3,1) NULL,
	[review_date] [datetime] NULL,
	[image_url] [varchar](500) NULL,
	[desc1] [varchar](7000) NULL,
	[desc2] [varchar](7000) NULL,
	[comment_text] [varchar](500) NULL,
	[photo_urls] [varchar](500) NULL
 CONSTRAINT [PK_profile_review] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------

SET IDENTITY_INSERT dbo.profile_review ON

-- PROFILE~REVIEW' => 8021
insert into [dbo].[profile_review] (id, entity_id, profile_type, profile_subtype, eo_type, eo_subtype, reviewed_by, stars, review_date, desc1, image_url, desc2, comment_text, photo_urls)
					select isn, eo_isn, profile_type, profile_subtype, eo_type, eo_subtype, profile1, profile2, profile8, profile6, profile7, 
					IIF(CHARINDEX('row', profile9)>0, CONVERT(xml, profile9).value('(/row/@desc2)[1]', 'varchar(4000)'), profile9) as desc2,
					IIF(CHARINDEX('row', profile9)>0, CONVERT(xml, profile9).value('(/row/@comment_text)[1]', 'varchar(4000)'), '') as comment_text,
					IIF(CHARINDEX('row', profile9)>0, CONVERT(xml, profile9).value('(/row/@photo_urls)[1]', 'varchar(4000)'), '') as photo_urls
from profiles where profile_type in (8021)

SET IDENTITY_INSERT dbo.profile_review OFF

--	select * from profile_review