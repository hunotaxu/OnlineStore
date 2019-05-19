/*
   Saturday, May 18, 20192:04:06 PM
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
CREATE TABLE dbo.Tmp_Category
	(
	Id int NOT NULL IDENTITY (1, 1),
	Name nvarchar(200) NOT NULL,
	Timestamp timestamp NULL,
	IsDeleted bit NOT NULL,
	Status tinyint NULL,
	DateCreated datetime NULL,
	DateModified datetime NULL,
	SortOrder tinyint NULL,
	ParentId int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Category SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Category ON
GO
IF EXISTS(SELECT * FROM dbo.Category)
	 EXEC('INSERT INTO dbo.Tmp_Category (Id, Name, IsDeleted, Status, DateCreated, DateModified, SortOrder, ParentId)
		SELECT Id, Name, IsDeleted, Status, DateCreated, DateModified, CONVERT(tinyint, SortOrder), ParentId FROM dbo.Category WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Category OFF
GO
DROP TABLE dbo.Category
GO
EXECUTE sp_rename N'dbo.Tmp_Category', N'Category', 'OBJECT' 
GO
ALTER TABLE dbo.Category ADD CONSTRAINT
	PK_Category PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
select Has_Perms_By_Name(N'dbo.Category', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Category', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Category', 'Object', 'CONTROL') as Contr_Per 