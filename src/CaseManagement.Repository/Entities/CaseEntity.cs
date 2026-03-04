using System;

namespace CaseManagement.Repository.Entities
{
    /// <summary>
    /// 案件實體 (對應資料表 CASEM)
    /// </summary>
    public class CaseEntity
    {
        /// <summary>
        /// 案件編號 (主鍵，格式：yyyyMMdd-00001)
        /// </summary>
        public string CaseNumber { get; set; }

        /// <summary>
        /// 密碼 (6位數字)
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 案件日期
        /// </summary>
        public DateTime CaseDate { get; set; }

        /// <summary>
        /// 案件狀態 (收案、已回復)
        /// </summary>
        public string CaseStatus { get; set; }

        /// <summary>
        /// 案件主旨
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 案件內容
        /// </summary>
        public string Content { get; set; }
    }
}
