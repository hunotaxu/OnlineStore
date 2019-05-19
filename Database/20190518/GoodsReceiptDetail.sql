/*
   Saturday, May 18, 20192:25:40 PM
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
ALTER TABLE dbo.Event SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Event', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Event', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Event', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Item
	(
	Id int NOT NULL IDENTITY (1, 1),
	Timestamp timestamp NULL,
	Name nvarchar(100) NOT NULL,
	Price decimal(18, 2) NOT NULL,
	Description nvarchar(MAX) NULL,
	Image nvarchar(MAX) NULL,
	Quantity int NOT NULL,
	[View] int NULL,
	CategoryId int NOT NULL,
	AverageEvaluation decimal(18, 2) NULL,
	EventId int NULL,
	BrandName nvarchar(MAX) NULL,
	IsDeleted bit NOT NULL,
	Status tinyint NOT NULL,
	DateCreated datetime NULL,
	DateModified datetime NULL,
	PromotionPrice decimal(18, 2) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Item SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Item ON
GO
IF EXISTS(SELECT * FROM dbo.Item)
	 EXEC('INSERT INTO dbo.Tmp_Item (Id, Name, Price, Description, Image, Quantity, [View], CategoryId, AverageEvaluation, EventId, BrandName, IsDeleted, Status, DateCreated, DateModified, PromotionPrice)
		SELECT Id, Name, Price, Description, Image, Quantity, [View], CategoryId, AverageEvaluation, EventId, BrandName, IsDeleted, Status, DateCreated, DateModified, PromotionPrice FROM dbo.Item WITH (HOLDLOCK TABLOCKX)')
GO
COMMIT
