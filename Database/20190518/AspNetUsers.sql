/*
   Saturday, May 18, 20191:52:35 PM
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
ALTER TABLE dbo.AspNetUsers
	DROP CONSTRAINT DF_AspNetUsers_Id
GO
CREATE TABLE dbo.Tmp_AspNetUsers
	(
	Id uniqueidentifier NOT NULL,
	UserName nvarchar(256) NULL,
	NormalizedUserName nvarchar(256) NULL,
	Email nvarchar(256) NULL,
	NormalizedEmail nvarchar(256) NULL,
	EmailConfirmed bit NOT NULL,
	PasswordHash nvarchar(MAX) NULL,
	SecurityStamp nvarchar(MAX) NULL,
	ConcurrencyStamp nvarchar(MAX) NULL,
	PhoneNumber nvarchar(MAX) NULL,
	PhoneNumberConfirmed bit NOT NULL,
	TwoFactorEnabled bit NOT NULL,
	LockoutEnd datetimeoffset(7) NULL,
	LockoutEnabled bit NOT NULL,
	AccessFailedCount int NOT NULL,
	Name nvarchar(MAX) NULL,
	DOB datetime2(7) NOT NULL,
	Avatar nvarchar(MAX) NULL,
	Gender tinyint NOT NULL,
	DateCreated datetime2(7) NOT NULL,
	DateModified datetime2(7) NOT NULL,
	Status tinyint NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_AspNetUsers SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_AspNetUsers ADD CONSTRAINT
	DF_AspNetUsers_Id DEFAULT (newid()) FOR Id
GO
IF EXISTS(SELECT * FROM dbo.AspNetUsers)
	 EXEC('INSERT INTO dbo.Tmp_AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, Name, DOB, Avatar, Gender, DateCreated, DateModified, Status)
		SELECT Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, Name, DOB, Avatar, CONVERT(tinyint, Gender), DateCreated, DateModified, CONVERT(tinyint, Status) FROM dbo.AspNetUsers WITH (HOLDLOCK TABLOCKX)')
GO
ALTER TABLE dbo.Cart
	DROP CONSTRAINT FK_Cart_AspNetUsers
GO
ALTER TABLE dbo.UserAddress
	DROP CONSTRAINT FK_User_Address_AspNetUsers
GO
ALTER TABLE dbo.Comment
	DROP CONSTRAINT FK_Comment_AspNetUsers
GO
ALTER TABLE dbo.AspNetUserClaims
	DROP CONSTRAINT FK_AspNetUserClaims_AspNetUsers_UserId
GO
ALTER TABLE dbo.AspNetUserClaims
	DROP CONSTRAINT FK_AspNetUserClaims_AspNetUsers_UserId1
GO
ALTER TABLE dbo.AspNetUserLogins
	DROP CONSTRAINT FK_AspNetUserLogins_AspNetUsers_UserId
GO
ALTER TABLE dbo.AspNetUserLogins
	DROP CONSTRAINT FK_AspNetUserLogins_AspNetUsers_UserId1
GO
ALTER TABLE dbo.AspNetUserRoles
	DROP CONSTRAINT FK_AspNetUserRoles_AspNetUsers_UserId
GO
ALTER TABLE dbo.AspNetUserRoles
	DROP CONSTRAINT FK_AspNetUserRoles_AspNetUsers_UserId1
GO
ALTER TABLE dbo.AspNetUserTokens
	DROP CONSTRAINT FK_AspNetUserTokens_AspNetUsers_UserId
GO
ALTER TABLE dbo.AspNetUserTokens
	DROP CONSTRAINT FK_AspNetUserTokens_AspNetUsers_UserId1
GO
DROP TABLE dbo.AspNetUsers
GO
EXECUTE sp_rename N'dbo.Tmp_AspNetUsers', N'AspNetUsers', 'OBJECT' 
GO
ALTER TABLE dbo.AspNetUsers ADD CONSTRAINT
	PK_AspNetUsers PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX EmailIndex ON dbo.AspNetUsers
	(
	NormalizedEmail
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX UserNameIndex ON dbo.AspNetUsers
	(
	NormalizedUserName
	) WHERE ([NormalizedUserName] IS NOT NULL)
 WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
COMMIT
BEGIN TRANSACTION
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
ALTER TABLE dbo.AspNetUserTokens SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
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
ALTER TABLE dbo.AspNetUserRoles SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
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
ALTER TABLE dbo.AspNetUserLogins SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
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
ALTER TABLE dbo.AspNetUserClaims SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Comment ADD CONSTRAINT
	FK_Comment_AspNetUsers FOREIGN KEY
	(
	CustomerId
	) REFERENCES dbo.AspNetUsers
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Comment SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
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
ALTER TABLE dbo.UserAddress SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Cart ADD CONSTRAINT
	FK_Cart_AspNetUsers FOREIGN KEY
	(
	CustomerId
	) REFERENCES dbo.AspNetUsers
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Cart SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
