using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShowPets;
using ShowPets.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ShowPetsTest
{
    [TestClass]
    public class AppTest
    {
        private readonly Mock<ILogger<App>> logger;
        private readonly Mock<IFilterService> filterServiceMock;
        private readonly App app;
        private readonly StringWriter outputWriter;
        private readonly StringReader inputReader;
        private readonly string input = "a";

        public AppTest()
        {
            logger = new Mock<ILogger<App>>();
            filterServiceMock = new Mock<IFilterService>();
            outputWriter = new StringWriter();
            inputReader = new StringReader(input);
            app = new App(logger.Object, filterServiceMock.Object, outputWriter, inputReader);
        }

        [TestMethod]
        public void RunAsync_GetsData_ReturnsList()
        {
            // Arrange
            var dict = new Dictionary<string, List<string>>
            {
                { "Male", new List<string> { "Garfield","Jennifer" } }
            };
            filterServiceMock.Setup(m => m.FilterPets())
                .Returns(Task.FromResult(dict));

            // Act
            app.RunAsync().Wait();
            var result = outputWriter.ToString();

            // Assert
            var expected = $"Male{Environment.NewLine}{Environment.NewLine}" +
                $" * {dict["Male"][0]}{Environment.NewLine}" +
                $" * {dict["Male"][1]}{Environment.NewLine}" +
                $"{Environment.NewLine}{App.PressAnyKey}{Environment.NewLine}";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RunAsync_GetsNoData_ReturnsError()
        {
            // Arrange
            var dict = new Dictionary<string, List<string>>
            {
                { "Error", new List<string> { "No Data to Process" } }
            };
            filterServiceMock.Setup(m => m.FilterPets())
                .Returns(Task.FromResult(dict));

            // Act
            app.RunAsync().Wait();
            var result = outputWriter.ToString();

            // Assert
            var expected = "Error: " + dict["Error"][0] + Environment.NewLine
            + App.PressAnyKey + Environment.NewLine;

            Assert.AreEqual(expected, result);
        }

    }
}
