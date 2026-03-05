using System.Linq;
using System.Net;
using System.Web.Http;
using CaseManagement.Service.Interfaces;
using CaseManagement.Service.Models;
using CaseManagement.WebApi.Models;

namespace CaseManagement.WebApi.Controllers
{
    /// <summary>
    /// 案件管理 API 控制器
    /// </summary>
    [RoutePrefix("api/cases")]
    public class CasesController : ApiController
    {
        private readonly ICaseService _caseService;

        public CasesController(ICaseService caseService)
        {
            _caseService = caseService;
        }

        /// <summary>
        /// 建立案件
        /// POST api/cases
        /// </summary>
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateCase([FromBody] CreateCaseRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _caseService.CreateCase(request.Subject, request.Content);

            var response = new CreateCaseResponse
            {
                Success = result.Success,
                CaseNumber = result.CaseNumber,
                Password = result.Password,
                ErrorMessage = result.ErrorMessage
            };

            if (result.Success)
            {
                return Created($"api/cases/{result.CaseNumber}", response);
            }

            return Content(HttpStatusCode.InternalServerError, response);
        }

        // [已停用] GetAllCases 功能暫時停用
        // /// <summary>
        // /// 取得所有案件
        // /// GET api/cases
        // /// </summary>
        // [HttpGet]
        // [Route("")]
        // public IHttpActionResult GetAllCases()
        // {
        //     var cases = _caseService.GetAllCases();
        //     var response = cases.Select(MapToResponse);
        //     return Ok(response);
        // }

        /// <summary>
        /// 取得單一案件
        /// GET api/cases/{caseNumber}
        /// 需要在 Header 中提供 X-Case-Password
        /// </summary>
        [HttpGet]
        [Route("{caseNumber}")]
        public IHttpActionResult GetCase(string caseNumber)
        {
            // 從 Header 取得密碼
            var password = Request.Headers.Contains("X-Case-Password")
                ? Request.Headers.GetValues("X-Case-Password").FirstOrDefault()
                : null;

            // 密碼為必填
            if (string.IsNullOrEmpty(password))
            {
                return Content(HttpStatusCode.Unauthorized, new { Success = false, Message = "缺少案件密碼" });
            }

            var caseDto = _caseService.GetCase(caseNumber, password);

            if (caseDto == null)
            {
                return Content(HttpStatusCode.Unauthorized, new { Success = false, Message = "案件不存在或密碼錯誤" });
            }

            return Ok(MapToResponse(caseDto));
        }

        /// <summary>
        /// 更新案件
        /// PUT api/cases/{caseNumber}
        /// 需要在 Header 中提供 X-Case-Password
        /// </summary>
        [HttpPut]
        [Route("{caseNumber}")]
        public IHttpActionResult UpdateCase(string caseNumber, [FromBody] UpdateCaseRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 從 Header 取得密碼
            var password = Request.Headers.Contains("X-Case-Password")
                ? Request.Headers.GetValues("X-Case-Password").FirstOrDefault()
                : null;

            // 密碼為必填
            if (string.IsNullOrEmpty(password))
            {
                return Content(HttpStatusCode.Unauthorized, new { Success = false, Message = "缺少案件密碼" });
            }

            var existingCase = _caseService.GetCase(caseNumber, password);
            if (existingCase == null)
            {
                return Content(HttpStatusCode.Unauthorized, new { Success = false, Message = "案件不存在或密碼錯誤" });
            }

            var caseDto = new CaseDto
            {
                CaseNumber = caseNumber,
                Password = existingCase.Password,
                CaseDate = existingCase.CaseDate,
                CaseStatus = request.CaseStatus,
                Subject = request.Subject,
                Content = request.Content
            };

            var success = _caseService.UpdateCase(caseDto);

            if (success)
            {
                return Ok(new { Success = true, Message = "案件更新成功" });
            }

            return Content(HttpStatusCode.InternalServerError, new { Success = false, Message = "案件更新失敗" });
        }

        /// <summary>
        /// 刪除案件
        /// DELETE api/cases/{caseNumber}
        /// 需要在 Header 中提供 X-Case-Password
        /// </summary>
        [HttpDelete]
        [Route("{caseNumber}")]
        public IHttpActionResult DeleteCase(string caseNumber)
        {
            // 從 Header 取得密碼
            var password = Request.Headers.Contains("X-Case-Password")
                ? Request.Headers.GetValues("X-Case-Password").FirstOrDefault()
                : null;

            // 密碼為必填
            if (string.IsNullOrEmpty(password))
            {
                return Content(HttpStatusCode.Unauthorized, new { Success = false, Message = "缺少案件密碼" });
            }

            var existingCase = _caseService.GetCase(caseNumber, password);
            if (existingCase == null)
            {
                return Content(HttpStatusCode.Unauthorized, new { Success = false, Message = "案件不存在或密碼錯誤" });
            }

            var success = _caseService.DeleteCase(caseNumber);

            if (success)
            {
                return Ok(new { Success = true, Message = "案件刪除成功" });
            }

            return Content(HttpStatusCode.InternalServerError, new { Success = false, Message = "案件刪除失敗" });
        }

        private static CaseResponse MapToResponse(CaseDto dto)
        {
            return new CaseResponse
            {
                CaseNumber = dto.CaseNumber,
                CaseDate = dto.CaseDate,
                CaseStatus = dto.CaseStatus,
                Subject = dto.Subject,
                Content = dto.Content
            };
        }
    }
}
