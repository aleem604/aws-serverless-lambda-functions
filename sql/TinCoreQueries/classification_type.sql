
-- =========================================
-- Create ClassificationType table template
--EO_TYPE~CLASSIFICATIONTYPE 2421
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.classification_type', 'U') IS NOT NULL
  DROP TABLE dbo.classification_type
GO

CREATE TABLE [dbo].[classification_type](
	[id] [bigint] IDENTITY(500001,1) NOT NULL,
	[eo_type][smallint] NOT NULL,
	[eo_subtype][smallint] NOT NULL,
	[classification_id][bigint] NOT NULL,
	[name] [varchar](250) NULL,
	[full_name] [varchar](250) NULL,
	[image_url] [varchar](250) NULL,
	[desc1] [varchar](250) NULL,
	[status] [tinyint] NOT NULL DEFAULT ((1)),
	[status_change_date] [datetime] NULL,
	[status_changed_by] [bigint] NULL,
	[create_date] [datetime] NOT NULL,
	[created_by] [bigint] NOT NULL,
	[update_date] [datetime] NULL,
	[updated_by] [bigint] NOT NULL
 CONSTRAINT [PK_classification_type] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------

SET IDENTITY_INSERT dbo.classification_type ON

insert into [dbo].[classification_type] (id, eo_type, eo_subtype, classification_id, name, full_name, image_url, desc1, status, status_change_date, status_changed_by, create_date, created_by, update_date, updated_by)
select isn, eo_type, eo_subtype, hisn1, name1, long_name,name4,
CONVERT(xml, long_comment).value('(/row/@desc1)[1]', 'varchar(300)'),
 status, status_timestamp, status_changed_by_user, create_timestamp, created_by_user, last_update_timestamp, last_updated_by_user 
from eo where eo_type =  2421


SET IDENTITY_INSERT dbo.classification_type OFF

---  select * from classification_type order by 1 desc
