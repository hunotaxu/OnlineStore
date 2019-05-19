/*
   Saturday, May 18, 20192:09:12 PM
   User: sa
   Server: DESKTOP-L5BRUUB\SQLEXPRESS
   Database: OnlineStoreDB
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.[Order]
	DROP CONSTRAINT FK_Order_Address
GO
ALTER TABLE dbo.Address SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.[Order]
	DROP CONSTRAINT DF_Order_IsSelfReceive
GO
CREATE TABLE dbo.Tmp_Order
	(
	Id int NOT NULL IDENTITY (1, 1),
	Timestamp timestamp NOT NULL,
	DeliveryDate datetime2(7) NULL,
	Bonus decimal(18, 2) NULL,
	EmployeeId uniqueidentifier NULL,
	CustomerId uniqueidentifier NOT NULL,
	ShippingFee decimal(18, 2) NULL,
	Status tinyint NULL,
	OrderDate datetime2(7) NULL,
	AddressId int NULL,
	IsDeleted bit NOT NULL,
	DeliveryType tinyint NOT NULL,
	DateCreated datetime NULL,
	DateModified datetime NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Order SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_Order ADD CONSTRAINT
	DF_Order_IsSelfReceive DEFAULT ((0)) FOR DeliveryType
GO
SET IDENTITY_INSERT dbo.Tmp_Order ON
GO
IF EXISTS(SELECT * FROM dbo.[Order])
	 EXEC('INSERT INTO dbo.Tmp_Order (Id, DeliveryDate, Bonus, EmployeeId, CustomerId, ShippingFee, Status, OrderDate, AddressId, IsDeleted, DeliveryType, DateCreated, DateModified)
		SELECT Id, DeliveryDate, Bonus, EmployeeId, CustomerId, ShippingFee, CONVERT(tinyint, Status), OrderDate, AddressId, IsDeleted, CONVERT(tinyint, DeliveryType), DateCreated, DateModified FROM dbo.[Order] WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Order OFF
GO
ALTER TABLE dbo.LineItem
	DROP CONSTRAINT FK_LineItem_Order_OrderId
GO
DROP TABLE dbo.[Order]
GO
EXECUTE sp_rename N'dbo.Tmp_Order', N'Order', 'OBJECT' 
GO
ALTER TABLE dbo.[Order] ADD CONSTRAINT
	PK_Order PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_Order_CustomerId ON dbo.[Order]
	(
	CustomerId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE dbo.[Order] ADD CONSTRAINT
	FK_Order_Address FOREIGN KEY
	(
	AddressId
	) REFERENCES dbo.Address
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.LineItem ADD CONSTRAINT
	FK_LineItem_Order_OrderId FOREIGN KEY
	(
	OrderId
	) REFERENCES dbo.[Order]
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.LineItem SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
