/*
   Saturday, May 18, 20192:02:05 PM
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
ALTER TABLE dbo.AspNetRoles
	DROP CONSTRAINT DF_AspNetRoles_Id
GO
CREATE TABLE dbo.Tmp_AspNetRoles
	(
	Id uniqueidentifier NOT NULL,
	Name nvarchar(256) NULL,
	NormalizedName nvarchar(256) NULL,
	ConcurrencyStamp nvarchar(MAX) NULL,
	DateCreated datetime2(7) NOT NULL,
	DateModified datetime2(7) NOT NULL,
	Status tinyint NOT NULL,
	Description nvarchar(MAX) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_AspNetRoles SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_AspNetRoles ADD CONSTRAINT
	DF_AspNetRoles_Id DEFAULT (newid()) FOR Id
GO
IF EXISTS(SELECT * FROM dbo.AspNetRoles)
	 EXEC('INSERT INTO dbo.Tmp_AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp, DateCreated, DateModified, Status, Description)
		SELECT Id, Name, NormalizedName, ConcurrencyStamp, DateCreated, DateModified, CONVERT(tinyint, Status), Description FROM dbo.AspNetRoles WITH (HOLDLOCK TABLOCKX)')
GO
ALTER TABLE dbo.AspNetUserRoles
	DROP CONSTRAINT FK_AspNetUserRoles_AspNetRoles_RoleId
GO
ALTER TABLE dbo.AspNetUserRoles
	DROP CONSTRAINT FK_AspNetUserRoles_AspNetRoles_RoleId1
GO
ALTER TABLE dbo.AspNetRoleClaims
	DROP CONSTRAINT FK_AspNetRoleClaims_AspNetRoles_RoleId
GO
ALTER TABLE dbo.AspNetRoleClaims
	DROP CONSTRAINT FK_AspNetRoleClaims_AspNetRoles_RoleId1
GO
DROP TABLE dbo.AspNetRoles
GO
EXECUTE sp_rename N'dbo.Tmp_AspNetRoles', N'AspNetRoles', 'OBJECT' 
GO
ALTER TABLE dbo.AspNetRoles ADD CONSTRAINT
	PK_AspNetRoles PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE UNIQUE NONCLUSTERED INDEX RoleNameIndex ON dbo.AspNetRoles
	(
	NormalizedName
	) WHERE ([NormalizedName] IS NOT NULL)
 WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
COMMIT
select Has_Perms_By_Name(N'dbo.AspNetRoles', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AspNetRoles', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AspNetRoles', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.AspNetRoleClaims ADD CONSTRAINT
	FK_AspNetRoleClaims_AspNetRoles_RoleId FOREIGN KEY
	(
	RoleId
	) REFERENCES dbo.AspNetRoles
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.AspNetRoleClaims ADD CONSTRAINT
	FK_AspNetRoleClaims_AspNetRoles_RoleId1 FOREIGN KEY
	(
	RoleId1
	) REFERENCES dbo.AspNetRoles
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.AspNetRoleClaims SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.AspNetRoleClaims', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AspNetRoleClaims', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AspNetRoleClaims', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
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
ALTER TABLE dbo.AspNetUserRoles SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.AspNetUserRoles', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AspNetUserRoles', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AspNetUserRoles', 'Object', 'CONTROL') as Contr_Per 