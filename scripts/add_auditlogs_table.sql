-- SQL Script để tạo bảng AuditLogs cho FriedChickenDB
-- Chạy script này trong SQL Server Management Studio (SSMS)

USE [FriedChickenDB];
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditLogs]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AuditLogs](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [UserId] [int] NOT NULL,
        [UserName] [nvarchar](max) NOT NULL,
        [Action] [nvarchar](max) NOT NULL,
        [EntityName] [nvarchar](max) NOT NULL,
        [EntityId] [int] NOT NULL,
        [OldValue] [nvarchar](max) NOT NULL,
        [NewValue] [nvarchar](max) NOT NULL,
        [Note] [nvarchar](max) NULL,
        [CreatedAt] [datetime2](7) NOT NULL DEFAULT (GETUTCDATE()),
        [UpdatedAt] [datetime2](7) NULL,
        [IsDeleted] [bit] NOT NULL DEFAULT (0),
     CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
END
GO
