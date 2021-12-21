
-- =========================================
-- Create LocationClassification table template
--EO_TYPE~CLASSIFICATION  2420
--EO_TYPE~CLASSIFICATIONTYPE 2421
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.classification', 'U') IS NOT NULL
  DROP TABLE dbo.classification
GO

CREATE TABLE [dbo].[classification](
	[id] [bigint] IDENTITY(500001,1) NOT NULL,
	[eo_type][smallint] NOT NULL,
	[eo_subtype][smallint] NOT NULL,
	[parent_id][bigint] NOT NULL,
	[name] [varchar](250) NULL,
	[full_name] [varchar](250) NULL,
	[status] [tinyint] NOT NULL DEFAULT ((1)),
	[status_change_date] [datetime] NULL,
	[status_changed_by] [bigint] NULL,
	[create_date] [datetime] NOT NULL,
	[created_by] [bigint] NOT NULL,
	[update_date] [datetime] NULL,
	[updated_by] [bigint] NOT NULL
 CONSTRAINT [PK_classification] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------

SET IDENTITY_INSERT dbo.classification ON

insert into [dbo].[classification] (id, eo_type, eo_subtype, parent_id, name, full_name , status, status_change_date, status_changed_by, create_date, created_by, update_date, updated_by)
					select isn, eo_type, eo_subtype, hisn1, name1, long_name, status, status_timestamp, status_changed_by_user, create_timestamp, created_by_user, last_update_timestamp, last_updated_by_user 
from eo where eo_type =  2420


SET IDENTITY_INSERT dbo.classification OFF

---  select * from classification order by 1 desc
