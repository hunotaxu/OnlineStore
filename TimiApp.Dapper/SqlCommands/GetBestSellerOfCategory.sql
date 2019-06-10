drop procedure if exists GetBestSellerOfCategory
go
create proc GetBestSellerOfCategory
as
begin
	select TOP 3 c.[Name] as CategoryName,COUNT(*) as NumberOfDeliverdItems
	from dbo.[Order] as o, dbo.OrderItem as l, Item as i, Category as c
	where o.Id = l.OrderId
	and l.ItemId = i.Id
	and i.CategoryId = c.Id
	and o.Status = 4
	and o.IsDeleted = 0
	and i.IsDeleted = 0
	group by c.[Name]
	order by NumberOfDeliverdItems DESC
end
GO
exec GetBestSellerOfCategory