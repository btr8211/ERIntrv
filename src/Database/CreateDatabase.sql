-- =============================================
-- 案件管理系統資料庫建立腳本
-- 資料庫：CaseManagement
-- 資料表：CASEM
-- =============================================

-- 建立資料庫
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'CaseManagement')
BEGIN
    CREATE DATABASE [CaseManagement]
END
GO

USE [CaseManagement]
GO

-- =============================================
-- 建立案件資料表 CASEM
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CASEM]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[CASEM]
    (
        -- 案件編號 (主鍵，格式：yyyyMMdd-00001)
        [CaseNumber] NVARCHAR(14) NOT NULL,
        
        -- 密碼 (6位數字)
        [Password] NVARCHAR(6) NOT NULL,
        
        -- 案件日期
        [CaseDate] DATETIME NOT NULL,
        
        -- 案件狀態 (收案、已回復)
        [CaseStatus] NVARCHAR(10) NOT NULL,
        
        -- 案件主旨
        [Subject] NVARCHAR(200) NOT NULL,
        
        -- 案件內容
        [Content] NVARCHAR(4000) NOT NULL,
        
        -- 主鍵約束
        CONSTRAINT [PK_CASEM] PRIMARY KEY CLUSTERED ([CaseNumber] ASC)
    )
END
GO

-- =============================================
-- 建立索引
-- =============================================

-- 案件日期索引 (用於查詢當日案件數量)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = N'IX_CASEM_CaseDate' AND object_id = OBJECT_ID(N'[dbo].[CASEM]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_CASEM_CaseDate] 
    ON [dbo].[CASEM] ([CaseDate] ASC)
END
GO

-- 案件狀態索引 (用於依狀態查詢)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = N'IX_CASEM_CaseStatus' AND object_id = OBJECT_ID(N'[dbo].[CASEM]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_CASEM_CaseStatus] 
    ON [dbo].[CASEM] ([CaseStatus] ASC)
END
GO

-- =============================================
-- 建立預設約束
-- =============================================

-- 案件狀態預設值
IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE name = N'DF_CASEM_CaseStatus')
BEGIN
    ALTER TABLE [dbo].[CASEM] 
    ADD CONSTRAINT [DF_CASEM_CaseStatus] DEFAULT (N'收案') FOR [CaseStatus]
END
GO

-- 案件日期預設值
IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE name = N'DF_CASEM_CaseDate')
BEGIN
    ALTER TABLE [dbo].[CASEM] 
    ADD CONSTRAINT [DF_CASEM_CaseDate] DEFAULT (GETDATE()) FOR [CaseDate]
END
GO

-- =============================================
-- 建立檢查約束
-- =============================================

-- 案件狀態檢查約束
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = N'CK_CASEM_CaseStatus')
BEGIN
    ALTER TABLE [dbo].[CASEM] 
    ADD CONSTRAINT [CK_CASEM_CaseStatus] CHECK ([CaseStatus] IN (N'收案', N'已回復'))
END
GO

-- 密碼格式檢查約束 (6位數字)
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = N'CK_CASEM_Password')
BEGIN
    ALTER TABLE [dbo].[CASEM] 
    ADD CONSTRAINT [CK_CASEM_Password] CHECK (LEN([Password]) = 6 AND [Password] NOT LIKE '%[^0-9]%')
END
GO

PRINT '資料庫建立完成！'
GO
