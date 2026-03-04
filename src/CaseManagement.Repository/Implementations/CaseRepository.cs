using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using CaseManagement.Repository.Entities;
using CaseManagement.Repository.Interfaces;

namespace CaseManagement.Repository.Implementations
{
    /// <summary>
    /// 案件儲存庫實作
    /// </summary>
    public class CaseRepository : ICaseRepository
    {
        private readonly IDbConnection _connection;

        public CaseRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        /// <inheritdoc />
        public bool Insert(CaseEntity entity)
        {
            const string sql = @"
                INSERT INTO CASEM (CaseNumber, Password, CaseDate, CaseStatus, Subject, Content)
                VALUES (@CaseNumber, @Password, @CaseDate, @CaseStatus, @Subject, @Content)";

            var result = _connection.Execute(sql, entity);
            return result > 0;
        }

        /// <inheritdoc />
        public CaseEntity GetByCaseNumber(string caseNumber)
        {
            const string sql = @"
                SELECT CaseNumber, Password, CaseDate, CaseStatus, Subject, Content
                FROM CASEM
                WHERE CaseNumber = @CaseNumber";

            return _connection.QueryFirstOrDefault<CaseEntity>(sql, new { CaseNumber = caseNumber });
        }

        /// <inheritdoc />
        public IEnumerable<CaseEntity> GetAll()
        {
            const string sql = @"
                SELECT CaseNumber, Password, CaseDate, CaseStatus, Subject, Content
                FROM CASEM
                ORDER BY CaseDate DESC";

            return _connection.Query<CaseEntity>(sql);
        }

        /// <inheritdoc />
        public bool Update(CaseEntity entity)
        {
            const string sql = @"
                UPDATE CASEM
                SET Password = @Password,
                    CaseDate = @CaseDate,
                    CaseStatus = @CaseStatus,
                    Subject = @Subject,
                    Content = @Content
                WHERE CaseNumber = @CaseNumber";

            var result = _connection.Execute(sql, entity);
            return result > 0;
        }

        /// <inheritdoc />
        public bool Delete(string caseNumber)
        {
            const string sql = @"
                DELETE FROM CASEM
                WHERE CaseNumber = @CaseNumber";

            var result = _connection.Execute(sql, new { CaseNumber = caseNumber });
            return result > 0;
        }

        /// <inheritdoc />
        public int GetTodayCaseCount(DateTime date)
        {
            const string sql = @"
                SELECT COUNT(*)
                FROM CASEM
                WHERE CONVERT(DATE, CaseDate) = @Date";

            return _connection.ExecuteScalar<int>(sql, new { Date = date.Date });
        }
    }
}
