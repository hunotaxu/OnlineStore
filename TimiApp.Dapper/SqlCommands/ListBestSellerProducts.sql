use [OnlineStoreDb]
drop procedure if exists dbo.ListBestSellerProducts
GO
create procedure ListBestSellerProducts
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
	select Category.[Name] as CategoryName, Item.[Name] as ProductName, QuantityTotal, AmountTotal, RowsCount = count(*) over()
	from Item,(select ItemId, SUM(OrderItem.Quantity) as QuantityTotal, SUM(Amount) as AmountTotal 
	from OrderItem where IsDeleted = 0 and OrderId in (select Id from dbo.[Order] where Status = 4 and IsDeleted = 0
	and dbo.[Order].OrderDate >= cast(@fromDate as date) and dbo.[Order].OrderDate <= cast(@toDate as date)
	)
	group by ItemId) as listItem, Category
	where listItem.ItemId = Item.Id
	and Item.CategoryId = Category.Id 
	and Category.Id = (
	case
		when @categoryId = 0 then Category.Id
		else ISNULL(@categoryId, Category.Id)
	end)
	and Item.[Name] like '%'+ISNULL(@productName, '')+'%'
	and Item.IsDeleted = 0
	order by AmountTotal DESC, QuantityTotal DESC
	offset @startIndex rows
	fetch next @pageSize rows only
end
GO