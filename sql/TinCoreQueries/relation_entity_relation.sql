
-- =========================================
-- Create relation table template
-- 2700	 RELATION~RELATION_RELATION
-- =========================================
USE ProManDB
GO

IF OBJECT_ID('dbo.relation_entity_relation', 'U') IS NOT NULL
  DROP TABLE dbo.relation_entity_relation
GO

CREATE TABLE [dbo].[relation_entity_relation](
	[id] [bigint] IDENTITY(1000001,1) NOT NULL,
	[entity1_id] [bigint] NOT NULL,
	[entity1_type] [smallint] NOT NULL,
	[entity1_subtype] [smallint] NOT NULL,
	[entity2_id] [bigint] NOT NULL,
	[entity2_type] [smallint] NOT NULL,
	[entity2_subtype] [smallint] NOT NULL,
	[relation_type] [smallint] NOT NULL,
	[relation_subtype] [smallint] NOT NULL,	
	[entity1_name] [varchar](50) NOT NULL,
	[entity2_name] [varchar](50) NOT NULL,
	[status] [tinyint] NOT NULL DEFAULT ((1)),
	[status_change_date] [datetime] NULL,
	[status_changed_by] [bigint] NULL,
	[create_date] [datetime] NOT NULL,
	[created_by] [bigint] NULL,
	[update_date] [datetime] NULL,
	[updated_by] [bigint] NULL
 CONSTRAINT [PK_relation_entity_relation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------ Insert Query --------------------

SET IDENTITY_INSERT dbo.relation_entity_relation ON

insert into [dbo].[relation_entity_relation] (id, entity1_id, entity1_type, entity1_subtype, entity2_id, entity2_type, entity2_subtype, relation_type, relation_subtype, 
entity1_name, entity2_name, status, status_change_date, status_changed_by, create_date, created_by, update_date, updated_by)
select isn,eo1_isn,eo1_type,eo1_subtype,eo2_isn,eo2_type,eo2_subtype,relation_type,relation_subtype,eo1_name,eo2_name,status,status_timestamp,status_changed_by_user,
create_timestamp,created_by_user,last_update_timestamp,last_updated_by_user
from relations where relation_type = 2700


SET IDENTITY_INSERT dbo.relation_entity_relation OFF

--  select * from relation_entity_relation order by 1 desc
