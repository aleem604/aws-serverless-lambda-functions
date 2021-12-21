
-- =========================================
-- Create ReviewCategory table template
--EO_TYPE~REVIEW_CATEGORY_LI 2425
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.category_review_detail', 'U') IS NOT NULL
  DROP TABLE dbo.category_review_detail
GO

CREATE TABLE [dbo].[category_review_detail](
	[id] [bigint] IDENTITY(500001,1) NOT NULL,
	[eo_type][smallint] NOT NULL,
	[eo_subtype][smallint] NOT NULL,
	[category_review_id][bigint] NOT NULL,
	[name] [varchar](250) NULL,
	[full_name] [varchar](500) NULL,
	[description] [varchar](500) NULL,	
	[status] [tinyint] NOT NULL DEFAULT ((1)),
	[status_change_date] [datetime] NULL,
	[status_changed_by] [bigint] NULL,
	[create_date] [datetime] NOT NULL,
	[created_by] [bigint] NOT NULL,
	[update_date] [datetime] NULL,
	[updated_by] [bigint] NOT NULL
 CONSTRAINT [PK_category_review_detail] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------

SET IDENTITY_INSERT dbo.category_review_detail ON

insert into [dbo].[category_review_detail] (id, eo_type, eo_subtype, category_review_id, name, full_name, description, status, status_change_date, status_changed_by, create_date, created_by, update_date, updated_by)
					select isn, eo_type, eo_subtype, hisn1, name1, long_name, description, status, status_timestamp, status_changed_by_user, create_timestamp, created_by_user, last_update_timestamp, last_updated_by_user 
from eo where eo_type =  2425

SET IDENTITY_INSERT dbo.category_review_detail OFF

---  select * from category_review_detail order by 1 desc

--   select * from classification where id= 6140
