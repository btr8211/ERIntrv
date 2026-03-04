using System;
using Moq;
using NUnit.Framework;
using CaseManagement.Repository.Interfaces;
using CaseManagement.Service.Implementations;

namespace CaseManagement.Tests.Services
{
    [TestFixture]
    public class CaseNumberGeneratorTests
    {
        private Mock<ICaseRepository> _mockRepository;
        private CaseNumberGenerator _generator;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<ICaseRepository>();
            _generator = new CaseNumberGenerator(_mockRepository.Object);
        }

        [Test]
        public void Generate_FirstCaseOfDay_ShouldReturnSequence00001()
        {
            // Arrange
            var date = new DateTime(2026, 3, 3);
            _mockRepository.Setup(x => x.GetTodayCaseCount(date))
                          .Returns(0);

            // Act
            var result = _generator.Generate(date);

            // Assert
            Assert.AreEqual("20260303-00001", result);
        }

        [Test]
        public void Generate_SecondCaseOfDay_ShouldReturnSequence00002()
        {
            // Arrange
            var date = new DateTime(2026, 3, 3);
            _mockRepository.Setup(x => x.GetTodayCaseCount(date))
                          .Returns(1);

            // Act
            var result = _generator.Generate(date);

            // Assert
            Assert.AreEqual("20260303-00002", result);
        }

        [Test]
        public void Generate_TenthCaseOfDay_ShouldReturnSequence00010()
        {
            // Arrange
            var date = new DateTime(2026, 3, 3);
            _mockRepository.Setup(x => x.GetTodayCaseCount(date))
                          .Returns(9);

            // Act
            var result = _generator.Generate(date);

            // Assert
            Assert.AreEqual("20260303-00010", result);
        }

        [Test]
        public void Generate_DifferentDate_ShouldUseDatePrefix()
        {
            // Arrange
            var date = new DateTime(2026, 12, 25);
            _mockRepository.Setup(x => x.GetTodayCaseCount(date))
                          .Returns(0);

            // Act
            var result = _generator.Generate(date);

            // Assert
            Assert.AreEqual("20261225-00001", result);
        }

        [Test]
        public void Generate_ShouldCallRepositoryWithCorrectDate()
        {
            // Arrange
            var date = new DateTime(2026, 3, 3);
            _mockRepository.Setup(x => x.GetTodayCaseCount(date))
                          .Returns(0);

            // Act
            _generator.Generate(date);

            // Assert
            _mockRepository.Verify(x => x.GetTodayCaseCount(date), Times.Once);
        }
    }
}
