drop procedure if exists GetRevenueDaily
go
create PROC GetRevenueDaily
	@fromDate VARCHAR(10),
	@toDate VARCHAR(10)
AS
BEGIN
		  select
				CAST(@fromDate as date) as FromDate,
				CAST(@toDate as date) as ToDate,
                CAST(o.DateCreated AS DATE) as Date,
                --sum(oi.Quantity*oi.Price) as Revenue,
				sum(o.Total) as Revenue,
                --sum((oi.Quantity*oi.Price)-(oi.Quantity * i.OriginalPrice)) as Profit
				sum(o.Total - (oi.Quantity * i.OriginalPrice)) as Profit
                from dbo.[Order] o
                inner join dbo.OrderItem oi
                on o.Id = oi.OrderId
                inner join Item i
                on oi.ItemId  = i.Id
                where o.DateCreated <= cast(@toDate as date) 
				AND o.DateCreated >= cast(@fromDate as date)
				and o.Status = 4
				and o.IsDeleted = 0
				and i.IsDeleted = 0
				and oi.IsDeleted = 0
                group by o.DateCreated
END
go
select * from dbo.[Order]
EXEC dbo.GetRevenueDaily @fromDate = '20190501',
                         @toDate = '20190530' 
