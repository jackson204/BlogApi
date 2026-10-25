# BlogApi：從零做一個部落格 Web API

一邊做部落格 Web API，一邊學 .NET 後端的學習專案。
以部落格的功能當主線，每個階段遇到問題時，再學對應的主題：Docker、資料表關聯、LINQ / EF Core、DI、查詢效能優化。

📖 **學習筆記（HTML 版）**：https://jackson204.github.io/BlogApi/
> 筆記放在 [`docs/`](docs/)，透過 GitHub Pages 發布。每一課都有圖解和練習題，參考答案預設摺疊，寫完再打開對照。

## 全景：發一篇文章，會經過哪些地方？

> 做一個部落格平台的後端 API。明天主管打開網站發一篇文章，後天再打開，**那篇文章必須還在**。

「後天還要在」這句話，會把你從前端一路帶到資料庫：

![全景圖：前端送出 POST /api/posts，經過 Controller、EF Core、SqlClient 到 Docker 裡的 SQL Server，再把 201 Created 送回前端](docs/images/overview.svg)

| 步驟 | 發生什麼 | 在哪一課學 |
|---|---|---|
| ① | 前端送出 HTTP Request：`POST /api/posts`，內容是文章的 JSON | 01 |
| ② | `PostsController` 把 JSON 轉成 `PostRequest` 物件，交給 EF Core | 01；DbContext 怎麼傳進 Controller 是 03 DI |
| ③ | EF Core 把「新增這個物件」翻成 `INSERT` SQL，透過 `Microsoft.Data.SqlClient` 送到資料庫 | 01；連線字串在 00 |
| ④ | SQL Server 執行並寫入磁碟，回傳新文章的編號 | 00 Docker、volume；01 Migration 建表 |
| ⑤ | 後端回傳 `201 Created` + 文章 JSON | 01 |

後天主管再打開網站，前端送出 `GET /api/posts`，走的是同一條路：EF Core 送出 `SELECT`，SQL Server 把存著的文章拿回來，再包成 Response 回給前端。

**要記住的一件事：** 不管中間疊了幾層（Controller、EF Core、SqlClient），最後跟資料庫溝通的永遠是 **SQL**。所以每一課都會「觀察 EF Core 產生的 SQL」。

## 技術

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core
- SQL Server 2022（Docker）

## 學習路線

```mermaid
flowchart LR
    S0["0 Docker<br>跑 MSSQL"] --> S1["1 文章 CRUD<br>EF Core 入門"]
    S1 --> S2["2 資料表關聯<br>一對多 / 一對一 / 多對多"]
    S2 --> S3["3 程式分層<br>DI"]
    S3 --> S4["4 效能優化<br>N+1、投影、分頁"]
    S4 --> S5["5 選修<br>JWT、驗證、測試"]
```

| 課程 | 主題 | 狀態 |
|---|---|---|
| [00 環境準備：用 Docker 跑 MSSQL](https://jackson204.github.io/BlogApi/lessons/00-docker-mssql.html) | Docker、image / container、port、volume、連線字串 | ✅ |
| [01 文章 CRUD：EF Core 入門](https://jackson204.github.io/BlogApi/lessons/01-efcore-crud.html) | DbContext、Migration、LINQ ↔ SQL、User Secrets | ✅ |
| 02a 作者與文章：一對多 | Foreign Key、`Include` | ⏳ |
| 02b 使用者與個人檔案：一對一 | `HasOne` / `WithOne` | ⏳ |
| 02c 文章與標籤：多對多 | 中介表 | ⏳ |
| 03 程式分層與 DI | 沒有 DI 會怎樣、生命週期 | ⏳ |
| 04 文章列表效能優化 | N+1、延遲載入、`Select` 投影、`SelectMany`、`AsNoTracking`、分頁 | ⏳ |
| 05 選修 | JWT、FluentValidation、錯誤處理、單元測試 | ⏳ |

## 專案結構

```
BlogApi/
├─ README.md
├─ BlogApi.sln
├─ BlogApi/          ← Web API 專案（第 1 課建立）
└─ docs/             ← 學習筆記（GitHub Pages）
    ├─ index.html    ← 課程目錄，可搜尋
    ├─ assets/
    └─ lessons/
```

## 本機執行

需要：[.NET 10 SDK](https://dotnet.microsoft.com/download)、[Docker Desktop](https://www.docker.com/products/docker-desktop/)

1. 啟動 SQL Server（詳細說明見第 0 課）

   ```powershell
   docker run -d --name blog-mssql `
     -e "ACCEPT_EULA=Y" `
     -e "MSSQL_SA_PASSWORD=<你的密碼>" `
     -p 1433:1433 `
     mcr.microsoft.com/mssql/server:2022-latest
   ```

2. 執行 API（第 1 課完成後補上）
