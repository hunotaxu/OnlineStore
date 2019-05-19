/*
   Saturday, May 18, 20192:02:24 PM
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
ALTER TABLE dbo.AspNetUserClaims
	DROP CONSTRAINT FK_AspNetUserClaims_AspNetUsers_UserId
GO
ALTER TABLE dbo.AspNetUserClaims
	DROP CONSTRAINT FK_AspNetUserClaims_AspNetUsers_UserId1
GO
ALTER TABLE dbo.AspNetUsers SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.AspNetUsers', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AspNetUsers', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AspNetUsers', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_AspNetUserClaims
	(
	Id int NOT NULL IDENTITY (1, 1),
	UserId uniqueidentifier NOT NULL,
	ClaimType nvarchar(MAX) NULL,
	ClaimValue nvarchar(MAX) NULL,
	DateCreated datetime2(7) NOT NULL,
	DateModified datetime2(7) NOT NULL,
	Status tinyint NOT NULL,
	UserId1 uniqueidentifier NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_AspNetUserClaims SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_AspNetUserClaims ON
GO
IF EXISTS(SELECT * FROM dbo.AspNetUserClaims)
	 EXEC('INSERT INTO dbo.Tmp_AspNetUserClaims (Id, UserId, ClaimType, ClaimValue, DateCreated, DateModified, Status, UserId1)
		SELECT Id, UserId, ClaimType, ClaimValue, DateCreated, DateModified, CONVERT(tinyint, Status), UserId1 FROM dbo.AspNetUserClaims WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_AspNetUserClaims OFF
GO
DROP TABLE dbo.AspNetUserClaims
GO
EXECUTE sp_rename N'dbo.Tmp_AspNetUserClaims', N'AspNetUserClaims', 'OBJECT' 
GO
ALTER TABLE dbo.AspNetUserClaims ADD CONSTRAINT
	PK_AspNetUserClaims PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_AspNetUserClaims_UserId ON dbo.AspNetUserClaims
	(
	UserId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_AspNetUserClaims_UserId1 ON dbo.AspNetUserClaims
	(
	UserId1
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE dbo.AspNetUserClaims ADD CONSTRAINT
	FK_AspNetUserClaims_AspNetUsers_UserId FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.AspNetUsers
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.AspNetUserClaims ADD CONSTRAINT
	FK_AspNetUserClaims_AspNetUsers_UserId1 FOREIGN KEY
	(
	UserId1
	) REFERENCES dbo.AspNetUsers
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.AspNetUserClaims', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AspNetUserClaims', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AspNetUserClaims', 'Object', 'CONTROL') as Contr_Per 