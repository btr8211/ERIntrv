# 案件管理系統資料庫設計文件

## 1. 資料庫資訊

| 項目 | 值 |
|------|-----|
| 資料庫名稱 | CaseManagement |
| Server | SERVER1 |
| IP | localhost |
| 帳號 | sqlaccount |
| 密碼 | sqlpassword |

## 2. 資料表設計

### 2.1 CASEM (案件主檔)

| 欄位名稱 | 資料類型 | 允許 NULL | 預設值 | 說明 |
|----------|----------|-----------|--------|------|
| CaseNumber | NVARCHAR(14) | 否 | - | 案件編號 (主鍵) |
| Password | NVARCHAR(6) | 否 | - | 密碼 (6位數字) |
| CaseDate | DATETIME | 否 | GETDATE() | 案件日期 |
| CaseStatus | NVARCHAR(10) | 否 | '收案' | 案件狀態 |
| Subject | NVARCHAR(200) | 否 | - | 案件主旨 |
| Content | NVARCHAR(4000) | 否 | - | 案件內容 |

## 3. 索引設計

| 索引名稱 | 類型 | 欄位 | 說明 |
|----------|------|------|------|
| PK_CASEM | Clustered | CaseNumber | 主鍵索引 |
| IX_CASEM_CaseDate | Non-Clustered | CaseDate | 日期查詢索引 |
| IX_CASEM_CaseStatus | Non-Clustered | CaseStatus | 狀態查詢索引 |

## 4. 約束設計

### 4.1 主鍵約束

```sql
CONSTRAINT [PK_CASEM] PRIMARY KEY CLUSTERED ([CaseNumber] ASC)
```

### 4.2 預設值約束

```sql
-- 案件狀態預設值
CONSTRAINT [DF_CASEM_CaseStatus] DEFAULT (N'收案') FOR [CaseStatus]

-- 案件日期預設值
CONSTRAINT [DF_CASEM_CaseDate] DEFAULT (GETDATE()) FOR [CaseDate]
```

### 4.3 檢查約束

```sql
-- 案件狀態檢查
CONSTRAINT [CK_CASEM_CaseStatus] CHECK ([CaseStatus] IN (N'收案', N'已回復'))

-- 密碼格式檢查 (6位數字)
CONSTRAINT [CK_CASEM_Password] CHECK (LEN([Password]) = 6 AND [Password] NOT LIKE '%[^0-9]%')
```

## 5. DDL 腳本

```sql
-- 建立資料庫
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'CaseManagement')
BEGIN
    CREATE DATABASE [CaseManagement]
END
GO

USE [CaseManagement]
GO

-- 建立案件資料表
CREATE TABLE [dbo].[CASEM]
(
    [CaseNumber] NVARCHAR(14) NOT NULL,
    [Password] NVARCHAR(6) NOT NULL,
    [CaseDate] DATETIME NOT NULL,
    [CaseStatus] NVARCHAR(10) NOT NULL,
    [Subject] NVARCHAR(200) NOT NULL,
    [Content] NVARCHAR(4000) NOT NULL,
    
    CONSTRAINT [PK_CASEM] PRIMARY KEY CLUSTERED ([CaseNumber] ASC)
)
GO

-- 建立索引
CREATE NONCLUSTERED INDEX [IX_CASEM_CaseDate] 
ON [dbo].[CASEM] ([CaseDate] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_CASEM_CaseStatus] 
ON [dbo].[CASEM] ([CaseStatus] ASC)
GO

-- 建立預設約束
ALTER TABLE [dbo].[CASEM] 
ADD CONSTRAINT [DF_CASEM_CaseStatus] DEFAULT (N'收案') FOR [CaseStatus]
GO

ALTER TABLE [dbo].[CASEM] 
ADD CONSTRAINT [DF_CASEM_CaseDate] DEFAULT (GETDATE()) FOR [CaseDate]
GO

-- 建立檢查約束
ALTER TABLE [dbo].[CASEM] 
ADD CONSTRAINT [CK_CASEM_CaseStatus] CHECK ([CaseStatus] IN (N'收案', N'已回復'))
GO

ALTER TABLE [dbo].[CASEM] 
ADD CONSTRAINT [CK_CASEM_Password] CHECK (LEN([Password]) = 6 AND [Password] NOT LIKE '%[^0-9]%')
GO
```

## 6. 連線字串

```xml
<connectionStrings>
    <add name="CaseDB" 
         connectionString="Server=SERVER1;Database=CaseManagement;User Id=sqlaccount;Password=sqlpassword;TrustServerCertificate=True;" 
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

## 7. 資料範例

| CaseNumber | Password | CaseDate | CaseStatus | Subject | Content |
|------------|----------|----------|------------|---------|---------|
| 20260303-00001 | 123456 | 2026-03-03 10:30:00 | 收案 | 系統問題回報 | 系統登入異常... |
| 20260303-00002 | 654321 | 2026-03-03 11:00:00 | 已回復 | 帳號申請 | 申請新帳號... |
