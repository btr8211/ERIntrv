using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using CaseManagement.Service.Interfaces;
using CaseManagement.Service.Models;
using CaseManagement.WebApi.Controllers;
using CaseManagement.WebApi.Models;

namespace CaseManagement.Tests.Controllers
{
    [TestFixture]
    public class CasesControllerTests
    {
        private Mock<ICaseService> _mockCaseService;
        private CasesController _controller;

        [SetUp]
        public void Setup()
        {
            _mockCaseService = new Mock<ICaseService>();
            _controller = new CasesController(_mockCaseService.Object);
        }

        [Test]
        public void CreateCase_WithValidRequest_ShouldReturnCreated()
        {
            // Arrange
            var request = new CreateCaseRequest
            {
                Subject = "測試主旨",
                Content = "測試內容"
            };

            var serviceResult = CreateCaseResult.SuccessResult("20260303-00001", "123456");

            _mockCaseService.Setup(x => x.CreateCase(request.Subject, request.Content))
                           .Returns(serviceResult);

            // Act
            var result = _controller.CreateCase(request);

            // Assert
            Assert.IsInstanceOf<CreatedNegotiatedContentResult<CreateCaseResponse>>(result);
            var createdResult = (CreatedNegotiatedContentResult<CreateCaseResponse>)result;
            Assert.IsTrue(createdResult.Content.Success);
            Assert.AreEqual("20260303-00001", createdResult.Content.CaseNumber);
            Assert.AreEqual("123456", createdResult.Content.Password);
        }

        [Test]
        public void CreateCase_WhenServiceFails_ShouldReturnInternalServerError()
        {
            // Arrange
            var request = new CreateCaseRequest
            {
                Subject = "測試主旨",
                Content = "測試內容"
            };

            var serviceResult = CreateCaseResult.FailResult("案件建立失敗");

            _mockCaseService.Setup(x => x.CreateCase(request.Subject, request.Content))
                           .Returns(serviceResult);

            // Act
            var result = _controller.CreateCase(request);

            // Assert
            Assert.IsInstanceOf<NegotiatedContentResult<CreateCaseResponse>>(result);
            var contentResult = (NegotiatedContentResult<CreateCaseResponse>)result;
            Assert.AreEqual(HttpStatusCode.InternalServerError, contentResult.StatusCode);
            Assert.IsFalse(contentResult.Content.Success);
        }

        [Test]
        public void GetAllCases_ShouldReturnOkWithCases()
        {
            // Arrange
            var cases = new List<CaseDto>
            {
                new CaseDto
                {
                    CaseNumber = "20260303-00001",
                    CaseDate = DateTime.Now,
                    CaseStatus = "收案",
                    Subject = "測試主旨1",
                    Content = "測試內容1"
                },
                new CaseDto
                {
                    CaseNumber = "20260303-00002",
                    CaseDate = DateTime.Now,
                    CaseStatus = "收案",
                    Subject = "測試主旨2",
                    Content = "測試內容2"
                }
            };

            _mockCaseService.Setup(x => x.GetAllCases())
                           .Returns(cases);

            // Act
            var result = _controller.GetAllCases();

            // Assert
            Assert.IsInstanceOf<OkNegotiatedContentResult<IEnumerable<CaseResponse>>>(result);
            var okResult = (OkNegotiatedContentResult<IEnumerable<CaseResponse>>)result;
            Assert.AreEqual(2, okResult.Content.Count());
        }

        [Test]
        public void GetCase_WithExistingCaseNumber_ShouldReturnOk()
        {
            // Arrange
            var caseNumber = "20260303-00001";
            var caseDto = new CaseDto
            {
                CaseNumber = caseNumber,
                CaseDate = DateTime.Now,
                CaseStatus = "收案",
                Subject = "測試主旨",
                Content = "測試內容"
            };

            _mockCaseService.Setup(x => x.GetCase(caseNumber))
                           .Returns(caseDto);

            // Act
            var result = _controller.GetCase(caseNumber);

            // Assert
            Assert.IsInstanceOf<OkNegotiatedContentResult<CaseResponse>>(result);
            var okResult = (OkNegotiatedContentResult<CaseResponse>)result;
            Assert.AreEqual(caseNumber, okResult.Content.CaseNumber);
        }

        [Test]
        public void GetCase_WithNonExistingCaseNumber_ShouldReturnNotFound()
        {
            // Arrange
            var caseNumber = "20260303-99999";

            _mockCaseService.Setup(x => x.GetCase(caseNumber))
                           .Returns((CaseDto)null);

            // Act
            var result = _controller.GetCase(caseNumber);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void DeleteCase_WithExistingCase_ShouldReturnOk()
        {
            // Arrange
            var caseNumber = "20260303-00001";
            var caseDto = new CaseDto
            {
                CaseNumber = caseNumber,
                CaseDate = DateTime.Now,
                CaseStatus = "收案",
                Subject = "測試主旨",
                Content = "測試內容"
            };

            _mockCaseService.Setup(x => x.GetCase(caseNumber))
                           .Returns(caseDto);
            _mockCaseService.Setup(x => x.DeleteCase(caseNumber))
                           .Returns(true);

            // Act
            var result = _controller.DeleteCase(caseNumber);

            // Assert
            Assert.IsInstanceOf<OkNegotiatedContentResult<object>>(result);
        }

        [Test]
        public void DeleteCase_WithNonExistingCase_ShouldReturnNotFound()
        {
            // Arrange
            var caseNumber = "20260303-99999";

            _mockCaseService.Setup(x => x.GetCase(caseNumber))
                           .Returns((CaseDto)null);

            // Act
            var result = _controller.DeleteCase(caseNumber);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
