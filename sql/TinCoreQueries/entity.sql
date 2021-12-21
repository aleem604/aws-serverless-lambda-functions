
-- =========================================
-- Create entity table template
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.entity', 'U') IS NOT NULL
  DROP TABLE dbo.entity
GO

CREATE TABLE [dbo].[entity](
	[id] [bigint] IDENTITY(100001,1) NOT NULL,
	[eo_type][smallint] NOT NULL DEFAULT(1010),
	[eo_subtype][smallint] NOT NULL DEFAULT(12),
	[name] [varchar](250) NULL,
	[short_name] [varchar](500) NULL,
	[full_name] [varchar](250) NULL,
	[tag] [varchar](250) NULL,
	[description] [varchar](250) NULL,
	[desc1] [varchar](250) NULL,
	[desc2] [varchar](250) NULL,
	[desc3] [varchar](250) NULL,
	[video_url] [varchar](250) NULL,
	[status] [tinyint] NOT NULL DEFAULT ((1)),
	[status_change_date] [datetime] NULL,
	[status_changed_by] [bigint] NULL,
	[create_date] [datetime] NOT NULL,
	[created_by] [bigint] NOT NULL,
	[update_date] [datetime] NULL,
	[updated_by] [bigint] NULL
 CONSTRAINT [PK_entity] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------

SET IDENTITY_INSERT dbo.entity ON

insert into [dbo].[entity] (id, name, short_name, tag, full_name, description, desc1, desc2, desc3, video_url, status, status_change_date, status_changed_by,  create_date, created_by, update_date, updated_by)
					 select isn,name1, name1, name5,long_name,description, 
					 (CONVERT(xml, long_comment).value(N'(/row/@desc1)[1]', N'varchar(4000)')) as desc1,
					 (CONVERT(xml, long_comment).value(N'(/row/@desc2)[1]', N'varchar(4000)')) as desc2,
					 (CONVERT(xml, long_comment).value(N'(/row/@desc3)[1]', N'varchar(4000)')) as desc3,
					 (CONVERT(xml, long_comment).value(N'(/row/@video_url)[1]', N'varchar(4000)')) as video_url,
					 
					 status, status_timestamp, status_changed_by_user, create_timestamp, created_by_user, last_update_timestamp, last_updated_by_user 
from eo where eo_type = 1010
and eo_subtype = 12

SET IDENTITY_INSERT dbo.entity OFF

-- select * from dbo.entity where desc2 is not null
