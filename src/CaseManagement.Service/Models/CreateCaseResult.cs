namespace CaseManagement.Service.Models
{
    /// <summary>
    /// 建立案件結果
    /// </summary>
    public class CreateCaseResult
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

        /// <summary>
        /// 建立成功結果
        /// </summary>
        public static CreateCaseResult SuccessResult(string caseNumber, string password)
        {
            return new CreateCaseResult
            {
                Success = true,
                CaseNumber = caseNumber,
                Password = password
            };
        }

        /// <summary>
        /// 建立失敗結果
        /// </summary>
        public static CreateCaseResult FailResult(string errorMessage)
        {
            return new CreateCaseResult
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
