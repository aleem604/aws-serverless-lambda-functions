
-- =========================================
-- Create LocationProgram table template
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.location_program', 'U') IS NOT NULL
  DROP TABLE dbo.location_program
GO

CREATE TABLE [dbo].[location_program](
	[id] [bigint] IDENTITY(500001,1) NOT NULL,
	[eo_type] [smallint] NOT NULL,
	[eo_subtype] [smallint] NOT NULL,
	[location_id][bigint] NOT NULL DEFAULT(0),
	[name] [varchar](250) NULL,	
	[status] [tinyint] NOT NULL DEFAULT ((1)),
	[status_change_date] [datetime] NULL,
	[status_changed_by] [bigint] NULL,
	[create_date] [datetime] NOT NULL,
	[created_by] [bigint] NOT NULL,
	[update_date] [datetime] NULL,
	[updated_by] [bigint] NOT NULL
 CONSTRAINT [PK_location_program] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------

SET IDENTITY_INSERT dbo.location_program ON

insert into [dbo].[location_program] (id, eo_type, eo_subtype, location_id, name, status, status_change_date, status_changed_by,  create_date, created_by, update_date, updated_by)
					select isn, eo_type, eo_subtype, hisn1, name1, status, status_timestamp, status_changed_by_user, create_timestamp, created_by_user, last_update_timestamp, last_updated_by_user 
from eo where eo_type = 2412
and eo_subtype = 12


SET IDENTITY_INSERT dbo.location_program OFF

--  select * from location_program
