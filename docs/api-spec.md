# 案件管理系統 API 規格文件

## 1. API 概述

- **Base URL**: `/api`
- **格式**: JSON
- **編碼**: UTF-8

## 2. REST API 端點

### 2.1 建立案件

**POST** `/api/cases`

#### 請求

```json
{
    "subject": "案件主旨",
    "content": "案件內容"
}
```

| 欄位 | 類型 | 必填 | 說明 |
|------|------|------|------|
| subject | string | 是 | 案件主旨，最大200字 |
| content | string | 是 | 案件內容，最大4000字 |

#### 回應 - 成功 (201 Created)

```json
{
    "success": true,
    "caseNumber": "20260303-00001",
    "password": "123456"
}
```

#### 回應 - 失敗 (500 Internal Server Error)

```json
{
    "success": false,
    "errorMessage": "案件建立失敗"
}
```

---

### 2.2 取得所有案件

**GET** `/api/cases`

#### 回應 - 成功 (200 OK)

```json
[
    {
        "caseNumber": "20260303-00001",
        "caseDate": "2026-03-03 10:30:00",
        "caseStatus": "收案",
        "subject": "案件主旨",
        "content": "案件內容"
    }
]
```

---

### 2.3 取得單一案件

**GET** `/api/cases/{caseNumber}`

#### 路徑參數

| 參數 | 類型 | 說明 |
|------|------|------|
| caseNumber | string | 案件編號 |

#### 回應 - 成功 (200 OK)

```json
{
    "caseNumber": "20260303-00001",
    "caseDate": "2026-03-03 10:30:00",
    "caseStatus": "收案",
    "subject": "案件主旨",
    "content": "案件內容"
}
```

#### 回應 - 找不到 (404 Not Found)

```json
{
    "message": "No HTTP resource was found that matches the request URI"
}
```

---

### 2.4 更新案件

**PUT** `/api/cases/{caseNumber}`

#### 路徑參數

| 參數 | 類型 | 說明 |
|------|------|------|
| caseNumber | string | 案件編號 |

#### 請求

```json
{
    "caseStatus": "已回復",
    "subject": "更新後的主旨",
    "content": "更新後的內容"
}
```

| 欄位 | 類型 | 必填 | 說明 |
|------|------|------|------|
| caseStatus | string | 是 | 案件狀態 (收案/已回復) |
| subject | string | 是 | 案件主旨 |
| content | string | 是 | 案件內容 |

#### 回應 - 成功 (200 OK)

```json
{
    "success": true,
    "message": "案件更新成功"
}
```

#### 回應 - 找不到 (404 Not Found)

---

### 2.5 刪除案件

**DELETE** `/api/cases/{caseNumber}`

#### 路徑參數

| 參數 | 類型 | 說明 |
|------|------|------|
| caseNumber | string | 案件編號 |

#### 回應 - 成功 (200 OK)

```json
{
    "success": true,
    "message": "案件刪除成功"
}
```

#### 回應 - 找不到 (404 Not Found)

---

## 3. 錯誤處理規範

### 3.1 HTTP 狀態碼

| 狀態碼 | 說明 |
|--------|------|
| 200 | 成功 |
| 201 | 建立成功 |
| 400 | 請求參數錯誤 |
| 404 | 資源不存在 |
| 500 | 伺服器錯誤 |

### 3.2 驗證錯誤回應 (400 Bad Request)

```json
{
    "message": "The request is invalid.",
    "modelState": {
        "request.subject": ["案件主旨為必填"],
        "request.content": ["案件內容為必填"]
    }
}
```

### 3.3 系統錯誤回應 (500 Internal Server Error)

```json
{
    "success": false,
    "message": "系統發生錯誤，請稍後再試",
    "errorCode": "SYSTEM_ERROR"
}
```

## 4. 資料格式說明

### 4.1 案件編號格式

- 格式：`yyyyMMdd-NNNNN`
- 範例：`20260303-00001`
- 說明：日期 + 5位數流水號，每日重新計算

### 4.2 密碼格式

- 格式：6位數字
- 範例：`123456`
- 說明：系統自動產生的隨機密碼

### 4.3 案件狀態

| 狀態值 | 說明 |
|--------|------|
| 收案 | 案件已建立，等待處理 |
| 已回復 | 案件已處理完成 |
