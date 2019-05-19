/*
   Saturday, May 18, 20191:54:23 PM
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
ALTER TABLE dbo.AspNetRoleClaims
	DROP CONSTRAINT FK_AspNetRoleClaims_AspNetRoles_RoleId
GO
ALTER TABLE dbo.AspNetRoleClaims
	DROP CONSTRAINT FK_AspNetRoleClaims_AspNetRoles_RoleId1
GO
ALTER TABLE dbo.AspNetRoles SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_AspNetRoleClaims
	(
	Id int NOT NULL IDENTITY (1, 1),
	RoleId uniqueidentifier NOT NULL,
	ClaimType nvarchar(MAX) NULL,
	ClaimValue nvarchar(MAX) NULL,
	DateCreated datetime2(7) NOT NULL,
	DateModified datetime2(7) NOT NULL,
	Status tinyint NOT NULL,
	RoleId1 uniqueidentifier NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_AspNetRoleClaims SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_AspNetRoleClaims ON
GO
IF EXISTS(SELECT * FROM dbo.AspNetRoleClaims)
	 EXEC('INSERT INTO dbo.Tmp_AspNetRoleClaims (Id, RoleId, ClaimType, ClaimValue, DateCreated, DateModified, Status, RoleId1)
		SELECT Id, RoleId, ClaimType, ClaimValue, DateCreated, DateModified, CONVERT(tinyint, Status), RoleId1 FROM dbo.AspNetRoleClaims WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_AspNetRoleClaims OFF
GO
DROP TABLE dbo.AspNetRoleClaims
GO
EXECUTE sp_rename N'dbo.Tmp_AspNetRoleClaims', N'AspNetRoleClaims', 'OBJECT' 
GO
ALTER TABLE dbo.AspNetRoleClaims ADD CONSTRAINT
	PK_AspNetRoleClaims PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_AspNetRoleClaims_RoleId ON dbo.AspNetRoleClaims
	(
	RoleId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_AspNetRoleClaims_RoleId1 ON dbo.AspNetRoleClaims
	(
	RoleId1
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
COMMIT
