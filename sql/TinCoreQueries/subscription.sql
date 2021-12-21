
-- =========================================
-- Create EntitySubscription table templatec
--EO_TYPE~ENTITY_SUBSCRIPTION  2051
--
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.subscription', 'U') IS NOT NULL
  DROP TABLE dbo.subscription
GO

CREATE TABLE [dbo].[subscription](
	[id] [bigint] IDENTITY(1000001,1) NOT NULL,
	[eo_type] [smallint] NOT NULL,
	[eo_subtype] [smallint] NOT NULL,
	[subscription_template_id] [int] NOT NULL,
	--[name] [varchar](50) NULL,	
	[status] [tinyint] NOT NULL DEFAULT ((1)),
	[status_change_date] [datetime] NULL,
	[status_changed_by] [bigint] NULL,
	[create_date] [datetime] NOT NULL,
	[created_by] [bigint] NOT NULL,
	[update_date] [datetime] NULL,
	[updated_by] [bigint] NOT NULL
 CONSTRAINT [PK_entity_subscription] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------

SET IDENTITY_INSERT dbo.subscription ON

insert into [dbo].[subscription] (id, subscription_template_id, eo_type, eo_subtype, status,	status_change_date, status_changed_by, create_date, created_by, update_date, updated_by)
					select isn, hisn5, eo_type, eo_subtype, status,	status_timestamp, status_changed_by_user, create_timestamp, created_by_user, last_update_timestamp, last_updated_by_user
from eo where eo_type = 2051

SET IDENTITY_INSERT dbo.subscription OFF

--  select * from subscription order by 1 desc

--  select * from tracking order by 1 desc