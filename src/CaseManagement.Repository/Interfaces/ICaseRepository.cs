using System;
using System.Collections.Generic;
using CaseManagement.Repository.Entities;

namespace CaseManagement.Repository.Interfaces
{
    /// <summary>
    /// 案件儲存庫介面
    /// </summary>
    public interface ICaseRepository
    {
        /// <summary>
        /// 新增案件
        /// </summary>
        /// <param name="entity">案件實體</param>
        /// <returns>是否成功</returns>
        bool Insert(CaseEntity entity);

        /// <summary>
        /// 根據案件編號取得案件
        /// </summary>
        /// <param name="caseNumber">案件編號</param>
        /// <returns>案件實體</returns>
        CaseEntity GetByCaseNumber(string caseNumber);

        /// <summary>
        /// 取得所有案件
        /// </summary>
        /// <returns>案件列表</returns>
        IEnumerable<CaseEntity> GetAll();

        /// <summary>
        /// 更新案件
        /// </summary>
        /// <param name="entity">案件實體</param>
        /// <returns>是否成功</returns>
        bool Update(CaseEntity entity);

        /// <summary>
        /// 刪除案件
        /// </summary>
        /// <param name="caseNumber">案件編號</param>
        /// <returns>是否成功</returns>
        bool Delete(string caseNumber);

        /// <summary>
        /// 取得指定日期的案件數量
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>案件數量</returns>
        int GetTodayCaseCount(DateTime date);
    }
}
