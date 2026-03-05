using System.Collections.Generic;
using CaseManagement.Service.Models;

namespace CaseManagement.Service.Interfaces
{
    /// <summary>
    /// 案件服務介面
    /// </summary>
    public interface ICaseService
    {
        /// <summary>
        /// 建立案件
        /// </summary>
        /// <param name="subject">案件主旨</param>
        /// <param name="content">案件內容</param>
        /// <returns>建立結果</returns>
        CreateCaseResult CreateCase(string subject, string content);

        /// <summary>
        /// 根據案件編號取得案件
        /// </summary>
        /// <param name="caseNumber">案件編號</param>
        /// <param name="password">案件密碼</param>
        /// <returns>案件資料，若密碼不符則回傳 null</returns>
        CaseDto GetCase(string caseNumber, string password);

        /// <summary>
        /// 取得所有案件
        /// </summary>
        /// <returns>案件列表</returns>
        IEnumerable<CaseDto> GetAllCases();

        /// <summary>
        /// 更新案件
        /// </summary>
        /// <param name="caseDto">案件資料</param>
        /// <returns>是否成功</returns>
        bool UpdateCase(CaseDto caseDto);

        /// <summary>
        /// 刪除案件
        /// </summary>
        /// <param name="caseNumber">案件編號</param>
        /// <returns>是否成功</returns>
        bool DeleteCase(string caseNumber);
    }
}
