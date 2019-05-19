/*
   Saturday, May 18, 20192:03:26 PM
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
ALTER TABLE dbo.AspNetUserTokens
	DROP CONSTRAINT FK_AspNetUserTokens_AspNetUsers_UserId
GO
ALTER TABLE dbo.AspNetUserTokens
	DROP CONSTRAINT FK_AspNetUserTokens_AspNetUsers_UserId1
GO
ALTER TABLE dbo.AspNetUsers SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.AspNetUsers', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AspNetUsers', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AspNetUsers', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_AspNetUserTokens
	(
	UserId uniqueidentifier NOT NULL,
	LoginProvider nvarchar(450) NOT NULL,
	Name nvarchar(450) NOT NULL,
	Value nvarchar(MAX) NULL,
	DateCreated datetime2(7) NOT NULL,
	DateModified datetime2(7) NOT NULL,
	Status tinyint NOT NULL,
	UserId1 uniqueidentifier NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_AspNetUserTokens SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.AspNetUserTokens)
	 EXEC('INSERT INTO dbo.Tmp_AspNetUserTokens (UserId, LoginProvider, Name, Value, DateCreated, DateModified, Status, UserId1)
		SELECT UserId, LoginProvider, Name, Value, DateCreated, DateModified, CONVERT(tinyint, Status), UserId1 FROM dbo.AspNetUserTokens WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.AspNetUserTokens
GO
EXECUTE sp_rename N'dbo.Tmp_AspNetUserTokens', N'AspNetUserTokens', 'OBJECT' 
GO
ALTER TABLE dbo.AspNetUserTokens ADD CONSTRAINT
	PK_AspNetUserTokens PRIMARY KEY CLUSTERED 
	(
	UserId,
	LoginProvider,
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_AspNetUserTokens_UserId1 ON dbo.AspNetUserTokens
	(
	UserId1
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE dbo.AspNetUserTokens ADD CONSTRAINT
	FK_AspNetUserTokens_AspNetUsers_UserId FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.AspNetUsers
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.AspNetUserTokens ADD CONSTRAINT
	FK_AspNetUserTokens_AspNetUsers_UserId1 FOREIGN KEY
	(
	UserId1
	) REFERENCES dbo.AspNetUsers
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.AspNetUserTokens', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AspNetUserTokens', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AspNetUserTokens', 'Object', 'CONTROL') as Contr_Per 