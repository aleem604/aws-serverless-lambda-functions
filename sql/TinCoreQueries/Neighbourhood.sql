
-- =========================================
-- Create Country table template
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.neighbourhood', 'U') IS NOT NULL
  DROP TABLE dbo.neighbourhood
GO

CREATE TABLE [dbo].[neighbourhood](
	[id] [bigint] IDENTITY(800001,1) NOT NULL,
	[eo_type][smallint] NOT NULL,
	[eo_subtype][smallint] NOT NULL,
	[city_id][bigint] NOT NULL DEFAULT(0),
	[location_type] [int] NOT NULL,
	[name] [varchar](250) NOT NULL,
	[full_name] [varchar](250) NULL,
	[short_name] [varchar](50) NULL,
	[code] [varchar](50) NULL,
	[desc1] [varchar](1000) NULL,
	[desc2] [varchar](1000) NULL,
	[desc3] [varchar](1000) NULL,
	[image_url] [varchar](500) NULL,
	[featured] [varchar](500) NULL,
	[admin] [varchar](250) NULL,
	[country_meta] [varchar](250) NULL,
	[region_meta] [varchar](250) NULL,
	[city_meta] [varchar](250) NULL,
	[neighbourhood_meta] [varchar](250) NULL,
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
 CONSTRAINT [PK_neighbourhood] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------
declare @isn int =1723;
declare @hisn5 int = 1387
declare @eo_type int = 2410
declare @eo_subtype int = 12

SET IDENTITY_INSERT dbo.neighbourhood ON

insert into neighbourhood (id, eo_type, eo_subtype, city_id, name, full_name, short_name, code, location_type, 
		desc1, desc2, desc3, image_url, featured, admin, city_meta, country_meta, neighbourhood_meta, region_meta, site_owner, donotmiss_entities,featured_entities,status, create_date, created_by, update_date, updated_by)
select isn, eo_type, eo_subtype,hisn1, name1, long_name, name3, name2, hisn5, 
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

status, create_timestamp, created_by_user, last_update_timestamp, last_updated_by_user 
from eo where hisn5 = 1387
 and eo_type = 2410
 and eo_subtype = 12

SET IDENTITY_INSERT dbo.neighbourhood OFF

   --select * from neighbourhood

