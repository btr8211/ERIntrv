using System.ComponentModel.DataAnnotations;

namespace CaseManagement.WebApi.Models
{
    /// <summary>
    /// 建立案件請求模型
    /// </summary>
    public class CreateCaseRequest
    {
        /// <summary>
        /// 案件主旨
        /// </summary>
        [Required(ErrorMessage = "案件主旨為必填")]
        [StringLength(200, ErrorMessage = "案件主旨不可超過200字")]
        public string Subject { get; set; }

        /// <summary>
        /// 案件內容
        /// </summary>
        [Required(ErrorMessage = "案件內容為必填")]
        [StringLength(4000, ErrorMessage = "案件內容不可超過4000字")]
        public string Content { get; set; }
    }
}
