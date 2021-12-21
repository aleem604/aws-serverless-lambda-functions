
-- =========================================
-- Create PageBlock table templatec
-- EO_TYPE~PAGE_BLOCK  2340
--
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.page_block', 'U') IS NOT NULL
  DROP TABLE dbo.page_block
GO

CREATE TABLE [dbo].[page_block](
	[id] [bigint] IDENTITY(1000001,1) NOT NULL,
	[eo_type] [smallint] NOT NULL,
	[eo_subtype] [smallint] NOT NULL,	
	[parent_id][bigint] NOT NULL,
	[name] [varchar](250) NULL,
	[description] [varchar](250) NULL,
	
	[name1] [varchar](250) NULL,
	[name2] [varchar](250) NULL,
	[name3] [varchar](250) NULL,
	[name4] [varchar](250) NULL,
	[name5] [varchar](250) NULL,
	[name6] [varchar](250) NULL,
	[name7] [varchar](250) NULL,
	[name8] [varchar](250) NULL,
	[url1] [varchar](500) NULL,
	[url2] [varchar](500) NULL,
	[url3] [varchar](500) NULL,
	[url4] [varchar](500) NULL,
	[url5] [varchar](500) NULL,
	[url6] [varchar](500) NULL,
	[url7] [varchar](500) NULL,
	[url8] [varchar](500) NULL,

	[status] [tinyint] NOT NULL DEFAULT (1),
	[status_change_date] [datetime] NULL,
	[status_changed_by] [bigint] NULL,
	[create_date] [datetime] NOT NULL,
	[created_by] [bigint] NULL,
	[update_date] [datetime] NULL,
	[updated_by] [bigint] NULL

 CONSTRAINT [PK_page_block] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------

SET IDENTITY_INSERT dbo.page_block ON

insert into [dbo].[page_block] (id, eo_type, eo_subtype, parent_id, name, description, 
					name1, name2, name3, name4, name5, name6, name7, name8, 
					url1, url2, url3, url4, url5, url6, url7, url8,
					status,	status_change_date, status_changed_by, create_date, created_by, update_date, updated_by)

					select isn, eo_type, eo_subtype, hisn1, name1, description, 
					(CONVERT(xml, long_comment).value(N'(/row/@name1)[1]', N'varchar(4000)')) as name_1,
					(CONVERT(xml, long_comment).value(N'(/row/@name2)[1]', N'varchar(4000)')) as name_2,
					(CONVERT(xml, long_comment).value(N'(/row/@name3)[1]', N'varchar(4000)')) as name_3,
					(CONVERT(xml, long_comment).value(N'(/row/@name4)[1]', N'varchar(4000)')) as name_4,
					(CONVERT(xml, long_comment).value(N'(/row/@name5)[1]', N'varchar(4000)')) as name_5,
					(CONVERT(xml, long_comment).value(N'(/row/@name6)[1]', N'varchar(4000)')) as name_6,
					(CONVERT(xml, long_comment).value(N'(/row/@name7)[1]', N'varchar(4000)')) as name_7,
					(CONVERT(xml, long_comment).value(N'(/row/@name8)[1]', N'varchar(4000)')) as name_8,

					(CONVERT(xml, long_comment).value(N'(/row/@url1)[1]', N'varchar(4000)')) as url1,
					(CONVERT(xml, long_comment).value(N'(/row/@url2)[1]', N'varchar(4000)')) as url2,
					(CONVERT(xml, long_comment).value(N'(/row/@url3)[1]', N'varchar(4000)')) as url3,
					(CONVERT(xml, long_comment).value(N'(/row/@url4)[1]', N'varchar(4000)')) as url4,
					(CONVERT(xml, long_comment).value(N'(/row/@url5)[1]', N'varchar(4000)')) as url1,
					(CONVERT(xml, long_comment).value(N'(/row/@url6)[1]', N'varchar(4000)')) as url2,
					(CONVERT(xml, long_comment).value(N'(/row/@url7)[1]', N'varchar(4000)')) as url3,
					(CONVERT(xml, long_comment).value(N'(/row/@url8)[1]', N'varchar(4000)')) as url4,
					status, status_timestamp, status_changed_by_user, create_timestamp, created_by_user, last_update_timestamp, last_updated_by_user
from eo where eo_type = 2340


SET IDENTITY_INSERT dbo.page_block OFF

--      select * from page_block order by 1 desc
