using System;
using Moq;
using NUnit.Framework;
using CaseManagement.Common.Constants;
using CaseManagement.Repository.Entities;
using CaseManagement.Repository.Interfaces;
using CaseManagement.Service.Implementations;
using CaseManagement.Service.Interfaces;

namespace CaseManagement.Tests.Services
{
    [TestFixture]
    public class CaseServiceTests
    {
        private Mock<ICaseRepository> _mockRepository;
        private Mock<ICaseNumberGenerator> _mockNumberGenerator;
        private CaseService _caseService;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<ICaseRepository>();
            _mockNumberGenerator = new Mock<ICaseNumberGenerator>();
            _caseService = new CaseService(_mockRepository.Object, _mockNumberGenerator.Object);
        }

        [Test]
        public void CreateCase_WithValidInput_ShouldReturnSuccessResult()
        {
            // Arrange
            var subject = "測試主旨";
            var content = "測試內容";
            var expectedCaseNumber = "20260303-00001";

            _mockNumberGenerator.Setup(x => x.Generate(It.IsAny<DateTime>()))
                               .Returns(expectedCaseNumber);
            _mockRepository.Setup(x => x.Insert(It.IsAny<CaseEntity>()))
                          .Returns(true);

            // Act
            var result = _caseService.CreateCase(subject, content);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(expectedCaseNumber, result.CaseNumber);
            Assert.IsNotNull(result.Password);
            Assert.AreEqual(6, result.Password.Length);
        }

        [Test]
        public void CreateCase_WithEmptySubject_ShouldReturnFailResult()
        {
            // Arrange
            var subject = "";
            var content = "測試內容";

            // Act
            var result = _caseService.CreateCase(subject, content);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("案件主旨不可為空", result.ErrorMessage);
        }

        [Test]
        public void CreateCase_WithEmptyContent_ShouldReturnFailResult()
        {
            // Arrange
            var subject = "測試主旨";
            var content = "";

            // Act
            var result = _caseService.CreateCase(subject, content);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("案件內容不可為空", result.ErrorMessage);
        }

        [Test]
        public void CreateCase_WhenRepositoryFails_ShouldReturnFailResult()
        {
            // Arrange
            var subject = "測試主旨";
            var content = "測試內容";
            var expectedCaseNumber = "20260303-00001";

            _mockNumberGenerator.Setup(x => x.Generate(It.IsAny<DateTime>()))
                               .Returns(expectedCaseNumber);
            _mockRepository.Setup(x => x.Insert(It.IsAny<CaseEntity>()))
                          .Returns(false);

            // Act
            var result = _caseService.CreateCase(subject, content);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("案件建立失敗", result.ErrorMessage);
        }

        [Test]
        public void CreateCase_ShouldSetStatusToReceived()
        {
            // Arrange
            var subject = "測試主旨";
            var content = "測試內容";
            CaseEntity capturedEntity = null;

            _mockNumberGenerator.Setup(x => x.Generate(It.IsAny<DateTime>()))
                               .Returns("20260303-00001");
            _mockRepository.Setup(x => x.Insert(It.IsAny<CaseEntity>()))
                          .Callback<CaseEntity>(e => capturedEntity = e)
                          .Returns(true);

            // Act
            _caseService.CreateCase(subject, content);

            // Assert
            Assert.IsNotNull(capturedEntity);
            Assert.AreEqual(CaseStatus.Received, capturedEntity.CaseStatus);
        }

        [Test]
        public void GetCase_WithExistingCaseNumber_ShouldReturnCase()
        {
            // Arrange
            var caseNumber = "20260303-00001";
            var entity = new CaseEntity
            {
                CaseNumber = caseNumber,
                Password = "123456",
                CaseDate = DateTime.Now,
                CaseStatus = CaseStatus.Received,
                Subject = "測試主旨",
                Content = "測試內容"
            };

            _mockRepository.Setup(x => x.GetByCaseNumber(caseNumber))
                          .Returns(entity);

            // Act
            var result = _caseService.GetCase(caseNumber);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(caseNumber, result.CaseNumber);
        }

        [Test]
        public void GetCase_WithNonExistingCaseNumber_ShouldReturnNull()
        {
            // Arrange
            var caseNumber = "20260303-99999";

            _mockRepository.Setup(x => x.GetByCaseNumber(caseNumber))
                          .Returns((CaseEntity)null);

            // Act
            var result = _caseService.GetCase(caseNumber);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void DeleteCase_WithExistingCase_ShouldReturnTrue()
        {
            // Arrange
            var caseNumber = "20260303-00001";

            _mockRepository.Setup(x => x.Delete(caseNumber))
                          .Returns(true);

            // Act
            var result = _caseService.DeleteCase(caseNumber);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
