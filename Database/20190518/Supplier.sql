/*
   Saturday, May 18, 20192:11:09 PM
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
ALTER TABLE dbo.Supplier ADD
	DateCreated datetime NULL,
	DateModified datetime NULL,
	Status tinyint NULL
GO
ALTER TABLE dbo.Supplier SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Supplier', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Supplier', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Supplier', 'Object', 'CONTROL') as Contr_Per 