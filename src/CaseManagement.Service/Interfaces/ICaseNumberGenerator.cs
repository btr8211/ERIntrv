using System;

namespace CaseManagement.Service.Interfaces
{
    /// <summary>
    /// 案件編號產生器介面
    /// </summary>
    public interface ICaseNumberGenerator
    {
        /// <summary>
        /// 產生案件編號
        /// </summary>
        /// <param name="date">案件日期</param>
        /// <returns>案件編號 (格式：yyyyMMdd-00001)</returns>
        string Generate(DateTime date);
    }
}
