
-- =========================================
-- Create List item table template
--HIERARCHY~MENU 5100 
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.entity_menu', 'U') IS NOT NULL
  DROP TABLE dbo.entity_menu
GO

CREATE TABLE [dbo].[entity_menu](
	[id] [bigint] IDENTITY(100001,1) NOT NULL,
	[eo_type][smallint] NOT NULL,
	[eo_subtype][smallint] NOT NULL,
	[parent_id][bigint] NOT NULL,
	[list_id][bigint] NOT NULL,
	[location_id][bigint] NOT NULL,
	[name] [varchar](250) NULL,
	[full_name] [varchar](500) NULL,
	[description] [varchar](500) NULL,
	[status] [tinyint] NOT NULL DEFAULT ((1)),
	[status_change_date] [datetime] NULL,
	[status_changed_by] [bigint] NULL,
	[create_date] [datetime] NOT NULL,
	[created_by] [bigint] NULL,
	[update_date] [datetime] NULL,
	[updated_by] [bigint] NULL
 CONSTRAINT [PK_entity_menu] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------

SET IDENTITY_INSERT dbo.entity_menu ON

insert into [dbo].[entity_menu] (id, eo_type, eo_subtype, parent_id, list_id, location_id, name, full_name, description, status, status_change_date, status_changed_by, create_date, created_by, update_date, updated_by)
					select isn, eo_type, eo_subtype, hisn1, hisn2, hisn3, name1, long_name, description, status, status_timestamp, status_changed_by_user, create_timestamp, created_by_user, last_update_timestamp, last_updated_by_user 
from eo where eo_type =  5100

SET IDENTITY_INSERT dbo.entity_menu OFF

---  select * from list_item order by 1 desc

--   select * from eo where eo_type = 2531 order by 1 desc

--   select * from location where id = 2830

