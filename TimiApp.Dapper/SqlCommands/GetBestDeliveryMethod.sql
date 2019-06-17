USE [OnlineStoreDB]
GO
drop function if exists dbo.[TotalDeliveredOrder]
/****** Object:  UserDefinedFunction [dbo].[TotalDeliveredOrder]    Script Date: 2019-06-06 2:13:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[TotalDeliveredOrder]
()  
RETURNS int  
AS  
BEGIN  
       DECLARE @Result int  
       SELECT @Result = COUNT(*) from dbo.[Order] where Status = 4
       RETURN @Result  
END
GO
drop procedure if exists dbo.[GetBestDeliveryMethod]
go
create proc GetBestDeliveryMethod
as
begin
	DECLARE @total int  
	set @total = dbo.[TotalDeliveredOrder]()
	select TopPopularDeliveryMethods.ReceivingTypeName as receivingTypeName, 
	ROUND(CAST(TopPopularDeliveryMethods.NumberOfDeliveredOrder*100 as float)/(CAST(@total as float)), 2) as ProportionOfDeliverdItems
	from (select TOP 3 ReceivingType.Name as ReceivingTypeName, COUNT(*) as NumberOfDeliveredOrder
	--from dbo.[Order] inner join ReceivingType on ReceivingType.Id = dbo.[Order].ReceivingTypeId
	from dbo.[Order], ReceivingType
	where Status = 4 and dbo.[Order].IsDeleted = 0
	and ReceivingType.Id = dbo.[Order].ReceivingTypeId
	group by ReceivingType.Name
	order by NumberOfDeliveredOrder DESC) as TopPopularDeliveryMethods
end
Go

exec [GetBestDeliveryMethod]