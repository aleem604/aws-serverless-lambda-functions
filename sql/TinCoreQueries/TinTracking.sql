
-- =========================================
-- Create EntitySubscriptionTracking table templatec
-- EO_TYPE~SUBSCRIPTION  2051
--
-- =========================================
USE ProManDB
GO

--IF OBJECT_ID('dbo.tracking', 'U') IS NOT NULL
--  DROP TABLE dbo.tracking
--GO

--CREATE TABLE [dbo].[tracking](
--	[id] [bigint] IDENTITY(1000001,1) NOT NULL,
--	[entity_id] [bigint] NOT NULL,
--	[eo_type] [smallint] NOT NULL,
--	[eo_subtype] [smallint] NOT NULL,
--	[tracking_type] [smallint] NOT NULL,
--	[tracking_subtype] [smallint] NOT NULL,
--	[name] [varchar](50) NULL,	
--	[entity_subscription_id] [bigint] NOT NULL,	
--	[start_date] [datetime] NULL,
--	[end_date] [datetime] NULL,
--	[status] [tinyint] NOT NULL DEFAULT (1),
--	[status_change_date] [datetime] NULL,
--	[status_changed_by] [bigint] NULL,
--	[create_date] [datetime] NOT NULL,
--	[created_by] [bigint] NULL,
--	[update_date] [datetime] NULL,
--	[updated_by] [bigint] NULL
-- CONSTRAINT [PK_tracking] PRIMARY KEY CLUSTERED 
--(
--	[id] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--) ON [PRIMARY]
--GO

-------------------- Insert Query --------------------

--SET IDENTITY_INSERT dbo.tracking ON

--insert into [dbo].[tracking] (id, entity_id, eo_type, eo_subtype, tracking_type, tracking_subtype, name, entity_subscription_id, start_date, end_date, status, 
--					status_change_date, status_changed_by, create_date, created_by, update_date, updated_by)

--					select isn, eo_isn, eo_type, eo_subtype, tracking_type, tracking_subtype, name1, hisn1, start_datetime, end_datetime, status, 
--					status_timestamp, status_changed_by_user, create_timestamp, created_by_user, last_update_timestamp, last_updated_by_user
--from tracking_old where tracking_type = 2051

--SET IDENTITY_INSERT dbo.tracking OFF

--  select * from tracking order by 1 desc
