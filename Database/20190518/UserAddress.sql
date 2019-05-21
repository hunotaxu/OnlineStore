/*
   Sunday, May 19, 20193:08:27 PM
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
ALTER TABLE dbo.UserAddress
	DROP CONSTRAINT FK_User_Address_AspNetUsers
GO
ALTER TABLE dbo.AspNetUsers SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.UserAddress
	DROP CONSTRAINT FK_Customer_Address_Address
GO
ALTER TABLE dbo.Address SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_UserAddress
	(
	UserId uniqueidentifier NOT NULL,
	AddressId int NOT NULL,
	IsDeleted bit NOT NULL,
	PhoneNumber varchar(50) NOT NULL,
	AddressType tinyint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_UserAddress SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.UserAddress)
	 EXEC('INSERT INTO dbo.Tmp_UserAddress (UserId, AddressId, IsDeleted)
		SELECT UserId, AddressId, IsDeleted FROM dbo.UserAddress WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.UserAddress
GO
EXECUTE sp_rename N'dbo.Tmp_UserAddress', N'UserAddress', 'OBJECT' 
GO
ALTER TABLE dbo.UserAddress ADD CONSTRAINT
	PK_Customer_Address PRIMARY KEY CLUSTERED 
	(
	UserId,
	AddressId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.UserAddress ADD CONSTRAINT
	FK_Customer_Address_Address FOREIGN KEY
	(
	AddressId
	) REFERENCES dbo.Address
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.UserAddress ADD CONSTRAINT
	FK_User_Address_AspNetUsers FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.AspNetUsers
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
