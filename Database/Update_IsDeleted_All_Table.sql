alter table Cart drop column Deleted
alter table Cart add IsDeleted bit not null

alter table CartDetail drop column Deleted
alter table CartDetail add IsDeleted bit not null

alter table Category add IsDeleted bit not null CONSTRAINT DF_MyTable_MyColumn DEFAULT 0
ALTER TABLE dbo.Category DROP CONSTRAINT DF_MyTable_MyColumn

alter table Comment drop column Deleted
alter table Comment add IsDeleted bit not null

alter table Customer drop column Deleted
alter table Customer add IsDeleted bit not null

alter table Customer_Address add IsDeleted bit not null

alter table Event drop column Deleted
alter table Event add IsDeleted bit not null

alter table GoodsReceipt add IsDeleted bit not null CONSTRAINT DF_MyTable_MyColumn DEFAULT 0
ALTER TABLE dbo.GoodsReceipt DROP CONSTRAINT DF_MyTable_MyColumn

alter table GoodsReceiptDetail add IsDeleted bit not null CONSTRAINT DF_MyTable_MyColumn DEFAULT 0
ALTER TABLE dbo.GoodsReceiptDetail DROP CONSTRAINT DF_MyTable_MyColumn

alter table ImageEvent drop column Deleted
alter table ImageEvent add IsDeleted bit not null

alter table ImageProduct drop column Deleted
alter table ImageProduct add IsDeleted bit not null

alter table Item add IsDeleted bit not null CONSTRAINT DF_MyTable_MyColumn DEFAULT 0
ALTER TABLE dbo.Item DROP CONSTRAINT DF_MyTable_MyColumn

alter table LineItem drop column Deleted
alter table LineItem add IsDeleted bit not null

alter table [Order] drop column Deleted
alter table [Order] add IsDeleted bit not null

alter table Role drop column Deleted
alter table Role add IsDeleted bit not null

alter table Supplier add IsDeleted bit not null CONSTRAINT DF_MyTable_MyColumn DEFAULT 0
ALTER TABLE dbo.Supplier DROP CONSTRAINT DF_MyTable_MyColumn

alter table TypeOfCustomer add IsDeleted bit not null CONSTRAINT DF_MyTable_MyColumn DEFAULT 0
ALTER TABLE dbo.TypeOfCustomer DROP CONSTRAINT DF_MyTable_MyColumn

alter table TypeOfUser add IsDeleted bit not null CONSTRAINT DF_MyTable_MyColumn DEFAULT 0
ALTER TABLE dbo.TypeOfUser DROP CONSTRAINT DF_MyTable_MyColumn

alter table [User] drop column Deleted
alter table [User] add IsDeleted bit not null

alter table UserDecentralization drop column Deleted
alter table UserDecentralization add IsDeleted bit not null


