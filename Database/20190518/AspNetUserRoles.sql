/*
   Saturday, May 18, 20192:03:00 PM
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
ALTER TABLE dbo.AspNetUserRoles
	DROP CONSTRAINT FK_AspNetUserRoles_AspNetRoles_RoleId
GO
ALTER TABLE dbo.AspNetUserRoles
	DROP CONSTRAINT FK_AspNetUserRoles_AspNetRoles_RoleId1
GO
ALTER TABLE dbo.AspNetRoles SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.AspNetRoles', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AspNetRoles', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AspNetRoles', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.AspNetUserRoles
	DROP CONSTRAINT FK_AspNetUserRoles_AspNetUsers_UserId
GO
ALTER TABLE dbo.AspNetUserRoles
	DROP CONSTRAINT FK_AspNetUserRoles_AspNetUsers_UserId1
GO
ALTER TABLE dbo.AspNetUsers SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.AspNetUsers', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AspNetUsers', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AspNetUsers', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_AspNetUserRoles
	(
	UserId uniqueidentifier NOT NULL,
	RoleId uniqueidentifier NOT NULL,
	DateCreated datetime2(7) NOT NULL,
	DateModified datetime2(7) NOT NULL,
	Status tinyint NOT NULL,
	UserId1 uniqueidentifier NULL,
	RoleId1 uniqueidentifier NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_AspNetUserRoles SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.AspNetUserRoles)
	 EXEC('INSERT INTO dbo.Tmp_AspNetUserRoles (UserId, RoleId, DateCreated, DateModified, Status, UserId1, RoleId1)
		SELECT UserId, RoleId, DateCreated, DateModified, CONVERT(tinyint, Status), UserId1, RoleId1 FROM dbo.AspNetUserRoles WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.AspNetUserRoles
GO
EXECUTE sp_rename N'dbo.Tmp_AspNetUserRoles', N'AspNetUserRoles', 'OBJECT' 
GO
ALTER TABLE dbo.AspNetUserRoles ADD CONSTRAINT
	PK_AspNetUserRoles PRIMARY KEY CLUSTERED 
	(
	UserId,
	RoleId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_AspNetUserRoles_RoleId ON dbo.AspNetUserRoles
	(
	RoleId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_AspNetUserRoles_RoleId1 ON dbo.AspNetUserRoles
	(
	RoleId1
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_AspNetUserRoles_UserId1 ON dbo.AspNetUserRoles
	(
	UserId1
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE dbo.AspNetUserRoles ADD CONSTRAINT
	FK_AspNetUserRoles_AspNetUsers_UserId FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.AspNetUsers
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.AspNetUserRoles ADD CONSTRAINT
	FK_AspNetUserRoles_AspNetUsers_UserId1 FOREIGN KEY
	(
	UserId1
	) REFERENCES dbo.AspNetUsers
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.AspNetUserRoles ADD CONSTRAINT
	FK_AspNetUserRoles_AspNetRoles_RoleId FOREIGN KEY
	(
	RoleId
	) REFERENCES dbo.AspNetRoles
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.AspNetUserRoles ADD CONSTRAINT
	FK_AspNetUserRoles_AspNetRoles_RoleId1 FOREIGN KEY
	(
	RoleId1
	) REFERENCES dbo.AspNetRoles
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
select Has_Perms_By_Name(N'dbo.AspNetUserRoles', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AspNetUserRoles', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AspNetUserRoles', 'Object', 'CONTROL') as Contr_Per 