using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Domain.Services.Tests
{
    [TestClass()]
    public class InputServiceTests
    {
        private readonly InputService _inputService;
        private readonly Mock<ILogger<InputService>> _loggerMock;

        public InputServiceTests()
        {
            _loggerMock = new Mock<ILogger<InputService>>();
            _inputService = new InputService(_loggerMock.Object);
        }
        [TestMethod]
        public void GetCities_NoArgs_ReturnsCities()
        {
            // Arrange
            string[] args = [];
            IEnumerable<string> apiCities = new List<string> { "City1", "City2", "City3", "City4" };

            // Act
            var result = _inputService.GetCities(args, apiCities);

            // Assert
            Assert.AreEqual(4, result.Count());
            CollectionAssert.Contains(result.ToList(), "City1");
            CollectionAssert.Contains(result.ToList(), "City2");
            CollectionAssert.Contains(result.ToList(), "City3");
            CollectionAssert.Contains(result.ToList(), "City4");
        }

        [TestMethod]
        public void GetCities_WithValidArgs_ReturnsInputCities()
        {
            // Arrange
            string[] args = ["--cities", "City3", "City2"];
            IEnumerable<string> apiCities = new List<string> { "City1", "City2", "City3", "City4" };

            // Act
            var result = _inputService.GetCities(args, apiCities);

            // Assert
            Assert.AreEqual(2, result.Count());
            CollectionAssert.Contains(result.ToList(), "City2");
            CollectionAssert.Contains(result.ToList(), "City3");
        }

        [TestMethod]
        public void GetCities_WithInValidArgsParam_ThrowsArgumentException()
        {
            // Arrange
            string[] args = ["--NOTcities", "City3", "City2"];
            IEnumerable<string> apiCities = new List<string> { "City1", "City2", "City3", "City4" };

            // Act & Assert
            Assert.ThrowsException<ArgumentException>( () =>  _inputService.GetCities(args, apiCities));
        }

        [TestMethod]
        public void GetCities_WithInValidArgs_ThrowsArgumentException()
        {
            // Arrange
            string[] args = ["--cities", "City3,City2"];
            IEnumerable<string> apiCities = new List<string> { "City1", "City2", "City3", "City4" };

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentException>( () =>  _inputService.GetCities(args, apiCities));
            Assert.AreEqual("Cities not found in API: City3,City2", exception.Message);
        }
    }
}