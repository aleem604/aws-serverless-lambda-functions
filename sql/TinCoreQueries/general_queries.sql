select distinct relation_subtype from relations order by 1 desc

select * from relations where relation_type = 2710

select eo_type, (select top 1 codename from proman_codes where codeid = eo_type) as codname, 
count(eo_type) total from eo group by eo_type


SELECT * FROM PROMAN_CODES WHERE CODEID IN (SELECT EO_TYPE FROM EO) ORDER BY CODEID


select * from tracking where eo_type = 0

select * from (select tracking_type, (select top 1 codename from proman_codes where codeid = tracking_type) as codename, 
count(tracking_type) total from tracking group by tracking_type) as t where codename is not null and tracking_type > 0

