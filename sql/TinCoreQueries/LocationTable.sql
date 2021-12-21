
-- =========================================
-- Create Country table template
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.location', 'U') IS NOT NULL
  DROP TABLE dbo.location
GO

CREATE TABLE [dbo].[location](
	[id] [bigint] IDENTITY(10001,1) NOT NULL,
	[eo_type][smallint] NOT NULL,
	[eo_subtype][smallint] NOT NULL,
	[parent_id][bigint] NOT NULL DEFAULT(0),
	[location_type] [int] NOT NULL DEFAULT ((1384)),
	[name] [varchar](250) NULL,
	[full_name] [varchar](500) NULL,
	[short_name] [varchar](250) NULL,
	[code] [varchar](250) NULL,
	[desc1] [varchar](1000) NULL,
	[desc2] [varchar](1000) NULL,
	[desc3] [varchar](1000) NULL,
	[image_url] [varchar](500) NULL,
	[featured] [varchar](500) NULL,
	[admin] [varchar](250) NULL,
	[city] [varchar](250) NULL,
	[country] [varchar](250) NULL,
	[neighbourhood] [varchar](250) NULL,
	[region] [varchar](250) NULL,
	[site_owner] [varchar](250) NULL,
	[donotmiss_entities] [varchar](500) NULL,
	[featured_entities] [varchar](500) NULL,
	[status] [tinyint] NOT NULL DEFAULT ((1)),
	[status_change_date] [datetime] NULL,
	[status_changed_by] [bigint] NULL,
	[create_date] [datetime] NOT NULL,
	[created_by] [bigint] NULL,
	[update_date] [datetime] NULL,
	[updated_by] [bigint] NULL
 CONSTRAINT [PK_location] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------
declare @hisn5 int = 1384  -- country
declare @eo_type int = 2410
declare @eo_subtype int = 12

SET IDENTITY_INSERT dbo.location ON

insert into [dbo].[location] (id, eo_type, eo_subtype, parent_id, name, full_name, short_name, code, location_type, 
		desc1, desc2, desc3, image_url, featured, admin, city, country, neighbourhood, region, site_owner, donotmiss_entities,featured_entities,
		status, status_change_date, status_changed_by, create_date, created_by, update_date, updated_by)
select isn,eo_type, eo_subtype, hisn1, name1, long_name, name3, name2, hisn5, 
		CONVERT(xml, long_comment).value('(/row/@desc1)[1]', 'varchar(300)'),
		CONVERT(xml, long_comment).value('(/row/@desc2)[1]', 'varchar(300)'),
		CONVERT(xml, long_comment).value('(/row/@desc3)[1]', 'varchar(300)'),
		CONVERT(xml, long_comment).value('(/row/@image_url)[1]', 'varchar(300)'),
		CONVERT(xml, long_comment).value('(/row/@featured)[1]', 'varchar(300)'),
		CONVERT(xml, long_comment).value('(/row/@admin)[1]', 'varchar(300)'),
		CONVERT(xml, long_comment).value('(/row/@city)[1]', 'varchar(300)'),
		CONVERT(xml, long_comment).value('(/row/@country)[1]', 'varchar(300)'),
		CONVERT(xml, long_comment).value('(/row/@neighbourhood)[1]', 'varchar(300)'),
		CONVERT(xml, long_comment).value('(/row/@region)[1]', 'varchar(300)'),
		CONVERT(xml, long_comment).value('(/row/@site_owner)[1]', 'varchar(300)'),
		CONVERT(xml, long_comment).value('(/row/@donotmiss_entities)[1]', 'varchar(500)'),
		CONVERT(xml, long_comment).value('(/row/@featured_entities)[1]', 'varchar(500)'),

		status, status_timestamp, status_changed_by_user, create_timestamp, created_by_user, last_update_timestamp, last_updated_by_user 
		from eo where eo_type = 2410
		and eo_subtype = 12


SET IDENTITY_INSERT dbo.location OFF

-- select * from location