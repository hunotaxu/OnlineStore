use [OnlineStoreDb]
drop procedure if exists dbo.ListProductsHasNotBeenPurchased
GO
create procedure ListProductsHasNotBeenPurchased
(
	@fromDate varchar(20),
	@toDate varchar(20),
	@categoryId nvarchar(256),
	@productName nvarchar(256),
	@pageSize int,
	@pageIndex int
)
as
begin
	declare @startIndex int = (@pageIndex - 1) * @pageSize
	SET @fromDate = ISNULL(@fromDate, '17530101')
	SET @toDate = ISNULL(@toDate, GETDATE())
	select Category.[Name] as CategoryName, Item.[Name] as ProductName, RowsCount = COUNT(*) OVER()
	from Item
	inner join Category on Category.Id = Item.CategoryId
	where Item.IsDeleted = 0 
	and Category.Id = (
	case
		when @categoryId = 0 then Category.Id
		else ISNULL(@categoryId, Category.Id)
	end)
	and Item.Id != all
		(select ItemId
		from OrderItem 
		inner join dbo.[Order] on OrderItem.OrderId = dbo.[Order].Id
		where dbo.[Order].Status = 4 and dbo.[Order].IsDeleted = 0
		and dbo.[Order].OrderDate >= cast(@fromDate as date) and dbo.[Order].OrderDate <= cast(@toDate as date))
	and Item.[Name] like '%'+ISNULL(@productName, '')+'%'
	and Item.IsDeleted = 0
	order by Item.DateCreated
	offset @startIndex rows
	fetch next @pageSize rows only
end
GO