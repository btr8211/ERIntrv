namespace CaseManagement.WebApi.Models
{
    /// <summary>
    /// 建立案件回應模型
    /// </summary>
    public class CreateCaseResponse
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseNumber { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
