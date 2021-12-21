
-- =========================================
-- Create Category List table template
--EO_TYPE~LIST 2530	 
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.entity_list', 'U') IS NOT NULL
  DROP TABLE dbo.entity_list
GO

CREATE TABLE [dbo].[entity_list](
	[id] [bigint] IDENTITY(500001,1) NOT NULL,
	[eo_type][smallint] NOT NULL,
	[eo_subtype][smallint] NOT NULL,
	[entity_contact_id][bigint] NOT NULL,
	[parent_id][bigint] NOT NULL,
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
 CONSTRAINT [PK_list] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------

SET IDENTITY_INSERT dbo.entity_list ON

insert into [dbo].[entity_list] (id, eo_type, eo_subtype, entity_contact_id, parent_id, location_id, name, full_name, description, status, status_change_date, status_changed_by, create_date, created_by, update_date, updated_by)
					select isn, eo_type, eo_subtype, hisn1, hisn2, hisn3, name1, long_name, description, status, status_timestamp, status_changed_by_user, create_timestamp, created_by_user, last_update_timestamp, last_updated_by_user 
from eo where eo_type =  2530 and eo_subtype = 12

SET IDENTITY_INSERT dbo.entity_list OFF

---  select * from list order by 1 desc

