
-- =========================================
-- Create EntityContact table template
--EO_TYPE~ENTITY  1010
--
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.entity_contact', 'U') IS NOT NULL
  DROP TABLE dbo.entity_contact
GO

CREATE TABLE [dbo].[entity_contact](
	[id] [bigint] IDENTITY(1000001,1) NOT NULL,
	[entity_id][bigint] NOT NULL,
	[eo_type][smallint] NOT NULL DEFAULT(1010),
	[eo_subtype][smallint] NOT NULL DEFAULT(12),
	[contact_type][smallint] NOT NULL,
	[address] [varchar](500) NULL,
	[address2] [varchar](500) NULL,
	[city] [varchar](250) NULL,
	[state] [varchar](250) NULL,
	[country] [varchar](250) NULL,
	[phone] [varchar](250) NULL,
	[email] [varchar](250) NULL,
	[website] [varchar](5000) NULL,
	[status] [tinyint] NOT NULL DEFAULT ((1)),
	[status_change_date] [datetime] NULL,
	[status_changed_by] [bigint] NULL,
	[create_date] [datetime] NOT NULL,
	[created_by] [bigint] NOT NULL,
	[update_date] [datetime] NULL,
	[updated_by] [bigint] NOT NULL
 CONSTRAINT [PK_entity_contact] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------

SET IDENTITY_INSERT dbo.entity_contact ON

insert into [dbo].[entity_contact] (id, entity_id, contact_type,address,address2,city,state,country,phone,email,website, status, status_change_date, status_changed_by, create_date, created_by, update_date, updated_by)
					select isn, eo_isn, contact_type,contact1, contact2, city,state, country, phone, contact7, contact8, status, status_timestamp, status_changed_by_user, create_timestamp, created_by_user, last_update_timestamp, last_updated_by_user 
from contact where eo_type = 1010
and eo_subtype = 12

SET IDENTITY_INSERT dbo.entity_contact OFF

--select * from entity_contact
