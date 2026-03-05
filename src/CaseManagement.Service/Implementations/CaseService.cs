using System;
using System.Collections.Generic;
using System.Linq;
using CaseManagement.Common.Constants;
using CaseManagement.Common.Helpers;
using CaseManagement.Repository.Entities;
using CaseManagement.Repository.Interfaces;
using CaseManagement.Service.Interfaces;
using CaseManagement.Service.Models;

namespace CaseManagement.Service.Implementations
{
    /// <summary>
    /// 案件服務實作
    /// </summary>
    public class CaseService : ICaseService
    {
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseNumberGenerator _caseNumberGenerator;

        public CaseService(ICaseRepository caseRepository, ICaseNumberGenerator caseNumberGenerator)
        {
            _caseRepository = caseRepository;
            _caseNumberGenerator = caseNumberGenerator;
        }

        /// <inheritdoc />
        public CreateCaseResult CreateCase(string subject, string content)
        {
            try
            {
                // 驗證輸入
                if (string.IsNullOrWhiteSpace(subject))
                {
                    return CreateCaseResult.FailResult("案件主旨不可為空");
                }

                if (string.IsNullOrWhiteSpace(content))
                {
                    return CreateCaseResult.FailResult("案件內容不可為空");
                }

                // 產生案件編號和密碼
                var caseDate = DateTime.Now;
                var caseNumber = _caseNumberGenerator.Generate(caseDate);
                var password = PasswordGenerator.Generate();

                // 建立案件實體
                var entity = new CaseEntity
                {
                    CaseNumber = caseNumber,
                    Password = password,
                    CaseDate = caseDate,
                    CaseStatus = CaseStatus.Received,
                    Subject = subject,
                    Content = content
                };

                // 儲存案件
                var success = _caseRepository.Insert(entity);

                if (success)
                {
                    return CreateCaseResult.SuccessResult(caseNumber, password);
                }

                return CreateCaseResult.FailResult("案件建立失敗");
            }
            catch (Exception ex)
            {
                return CreateCaseResult.FailResult($"系統錯誤：{ex.Message}");
            }
        }

        /// <inheritdoc />
        public CaseDto GetCase(string caseNumber, string password)
        {
            var entity = _caseRepository.GetByCaseNumber(caseNumber);
            if (entity == null)
            {
                return null;
            }

            // 驗證密碼是否相符
            if (entity.Password != password)
            {
                return null;
            }

            return MapToDto(entity);
        }

        /// <inheritdoc />
        public IEnumerable<CaseDto> GetAllCases()
        {
            var entities = _caseRepository.GetAll();
            return entities.Select(MapToDto);
        }

        /// <inheritdoc />
        public bool UpdateCase(CaseDto caseDto)
        {
            var entity = new CaseEntity
            {
                CaseNumber = caseDto.CaseNumber,
                Password = caseDto.Password,
                CaseDate = caseDto.CaseDate,
                CaseStatus = caseDto.CaseStatus,
                Subject = caseDto.Subject,
                Content = caseDto.Content
            };

            return _caseRepository.Update(entity);
        }

        /// <inheritdoc />
        public bool DeleteCase(string caseNumber)
        {
            return _caseRepository.Delete(caseNumber);
        }

        private static CaseDto MapToDto(CaseEntity entity)
        {
            return new CaseDto
            {
                CaseNumber = entity.CaseNumber,
                Password = entity.Password,
                CaseDate = entity.CaseDate,
                CaseStatus = entity.CaseStatus,
                Subject = entity.Subject,
                Content = entity.Content
            };
        }
    }
}
