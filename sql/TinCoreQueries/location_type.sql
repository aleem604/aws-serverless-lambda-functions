
-- =========================================
-- Create PageBlock table templatec
-- EO_TYPE~LOCATIONTYPE 2411
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.location_type', 'U') IS NOT NULL
  DROP TABLE dbo.location_type
GO

CREATE TABLE [dbo].[location_type](
	[id] [bigint] IDENTITY(10001,1) NOT NULL,
	[eo_type] [smallint] NOT NULL,
	[name] [varchar](250) NULL,
	[status] [tinyint] NOT NULL DEFAULT ((1)),
	[status_change_date] [datetime] NULL,
	[status_changed_by] [bigint] NULL DEFAULT (0),
	[create_date] [datetime] NOT NULL,
	[created_by] [bigint] NULL DEFAULT ((0)),
	[update_date] [datetime] NULL,
	[updated_by] [bigint] NULL DEFAULT ((0)),

 CONSTRAINT [PK_location_type] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------

SET IDENTITY_INSERT dbo.location_type ON

insert into [dbo].[location_type] (id, eo_type, name, status,	status_change_date, status_changed_by, create_date, created_by, update_date, updated_by)

select isn, eo_type, name1, status, status_timestamp, status_changed_by_user, create_timestamp, created_by_user, last_update_timestamp, last_updated_by_user
from eo where eo_type = 2411


SET IDENTITY_INSERT dbo.location_type OFF

--      select * from location_type
