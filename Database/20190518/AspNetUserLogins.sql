/*
   Saturday, May 18, 20192:02:43 PM
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
ALTER TABLE dbo.AspNetUserLogins
	DROP CONSTRAINT FK_AspNetUserLogins_AspNetUsers_UserId
GO
ALTER TABLE dbo.AspNetUserLogins
	DROP CONSTRAINT FK_AspNetUserLogins_AspNetUsers_UserId1
GO
ALTER TABLE dbo.AspNetUsers SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.AspNetUsers', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AspNetUsers', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AspNetUsers', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_AspNetUserLogins
	(
	LoginProvider nvarchar(450) NOT NULL,
	ProviderKey nvarchar(450) NOT NULL,
	ProviderDisplayName nvarchar(MAX) NULL,
	UserId uniqueidentifier NOT NULL,
	DateCreated datetime2(7) NOT NULL,
	DateModified datetime2(7) NOT NULL,
	Status tinyint NOT NULL,
	UserId1 uniqueidentifier NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_AspNetUserLogins SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.AspNetUserLogins)
	 EXEC('INSERT INTO dbo.Tmp_AspNetUserLogins (LoginProvider, ProviderKey, ProviderDisplayName, UserId, DateCreated, DateModified, Status, UserId1)
		SELECT LoginProvider, ProviderKey, ProviderDisplayName, UserId, DateCreated, DateModified, CONVERT(tinyint, Status), UserId1 FROM dbo.AspNetUserLogins WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.AspNetUserLogins
GO
EXECUTE sp_rename N'dbo.Tmp_AspNetUserLogins', N'AspNetUserLogins', 'OBJECT' 
GO
ALTER TABLE dbo.AspNetUserLogins ADD CONSTRAINT
	PK_AspNetUserLogins PRIMARY KEY CLUSTERED 
	(
	LoginProvider,
	ProviderKey
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_AspNetUserLogins_UserId ON dbo.AspNetUserLogins
	(
	UserId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_AspNetUserLogins_UserId1 ON dbo.AspNetUserLogins
	(
	UserId1
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE dbo.AspNetUserLogins ADD CONSTRAINT
	FK_AspNetUserLogins_AspNetUsers_UserId FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.AspNetUsers
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.AspNetUserLogins ADD CONSTRAINT
	FK_AspNetUserLogins_AspNetUsers_UserId1 FOREIGN KEY
	(
	UserId1
	) REFERENCES dbo.AspNetUsers
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.AspNetUserLogins', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AspNetUserLogins', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AspNetUserLogins', 'Object', 'CONTROL') as Contr_Per 