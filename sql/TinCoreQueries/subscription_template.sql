
-- =========================================
-- Create EntitySubscription table templatec
--
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.subscription_template', 'U') IS NOT NULL
  DROP TABLE dbo.subscription_template
GO

CREATE TABLE [dbo].[subscription_template](
	[id] [int] IDENTITY(101,1) NOT NULL,
	[name][varchar](250) NOT NULL,
	[status] [tinyint] NOT NULL DEFAULT ((1)),
	[status_change_date] [datetime] NULL,
	[status_changed_by] [bigint] NULL,
	[create_date] [datetime] NOT NULL,
	[created_by] [bigint] NOT NULL,
	[update_date] [datetime] NULL,
	[updated_by] [bigint] NOT NULL
 CONSTRAINT [PK_tracking_template] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------

SET IDENTITY_INSERT dbo.subscription_template ON

insert into [dbo].[subscription_template] (id, name, status, status_change_date, status_changed_by, create_date, created_by, update_date, updated_by)
select hisn5, name1, status, status_timestamp, status_changed_by_user, create_timestamp, created_by_user, last_update_timestamp, last_updated_by_user
from eo where eo_type = 2051
order by hisn5 asc


SET IDENTITY_INSERT dbo.subscription_template OFF

update subscription_template set status =2 where id not in (2,3,4)

--  select * from tracking_template
