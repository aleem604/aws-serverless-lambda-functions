
-- =========================================
-- Create Category Attribute table template
--EO_TYPE~ATTRIBUTE 2430
-- list of attributes 	 
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.attribute', 'U') IS NOT NULL
  DROP TABLE dbo.attribute
GO

CREATE TABLE [dbo].[attribute](
	[id] [bigint] IDENTITY(10001,1) NOT NULL,
	[eo_type][smallint] NOT NULL,
	[eo_subtype][smallint] NOT NULL,
	[name] [varchar](250) NULL,
	[seq_no][int] NULL,
	[status] [tinyint] NOT NULL DEFAULT ((1)),
	[status_change_date] [datetime] NULL,
	[status_changed_by] [bigint] NULL,
	[create_date] [datetime] NOT NULL,
	[created_by] [bigint] NOT NULL,
	[update_date] [datetime] NULL,
	[updated_by] [bigint] NOT NULL
 CONSTRAINT [PK_attribute] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------

SET IDENTITY_INSERT dbo.attribute ON

insert into [dbo].[attribute] (id, eo_type, eo_subtype, name, seq_no, status, status_change_date, status_changed_by, create_date, created_by, update_date, updated_by)
					select isn, eo_type, eo_subtype, name1, hisn5, status, status_timestamp, status_changed_by_user, create_timestamp, created_by_user, last_update_timestamp, last_updated_by_user 
from eo where eo_type =  2430

SET IDENTITY_INSERT dbo.attribute OFF

---  select * from attribute order by 1 desc

--	select * from tracking where id = 6665

