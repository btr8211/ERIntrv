using System;

namespace CaseManagement.Service.Models
{
    /// <summary>
    /// 案件資料傳輸物件
    /// </summary>
    public class CaseDto
    {
        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseNumber { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 案件日期
        /// </summary>
        public DateTime CaseDate { get; set; }

        /// <summary>
        /// 案件狀態
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
