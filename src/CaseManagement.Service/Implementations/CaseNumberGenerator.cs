using System;
using CaseManagement.Repository.Interfaces;
using CaseManagement.Service.Interfaces;

namespace CaseManagement.Service.Implementations
{
    /// <summary>
    /// 案件編號產生器實作
    /// </summary>
    public class CaseNumberGenerator : ICaseNumberGenerator
    {
        private readonly ICaseRepository _caseRepository;

        public CaseNumberGenerator(ICaseRepository caseRepository)
        {
            _caseRepository = caseRepository;
        }

        /// <inheritdoc />
        public string Generate(DateTime date)
        {
            // 格式：yyyyMMdd-00001
            var datePrefix = date.ToString("yyyyMMdd");
            var todayCount = _caseRepository.GetTodayCaseCount(date);
            var sequence = (todayCount + 1).ToString("D5");
            return $"{datePrefix}-{sequence}";
        }
    }
}
