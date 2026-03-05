幫我用.Net Framework 4.8,建立 WebApi 服務
架構上有以下需求
* DI Framework 方式實作
* 建立專案，程式開發語言請使用 C#
* 資料庫使用SQL SERVER，相關資訊下面提供
* WebApi 請依循 Rest API 風格建立 C.R.U.D 等功能
先做功能1 建立案件
* 使用者會傳入下列資訊 (案件主旨、案件內容)
* 接收到資料後，需要建立該案件的編號(格式：yyyyddmm-00001)、密碼(隨機6位數字)、案件日期，並寫入資料庫
* 回傳成功、失敗狀態、案件編號與密碼
資料表(CASEM)欄位
* 案件編號、密碼、案件日期、案件狀態(收案、已回復)
資料庫資訊(我之後會填入)
* Server： SERVER1
* IP： localhost
* 帳號： sqlaccount
* 密碼： sqlpassword