using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using ShowCats.Models;
using ShowCats.Services;
using ShowPets;
using ShowPets.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ShowPetsTest
{
    [TestClass]
    public class DataServiceTest
    {
        private Mock<ILogger<DataService>> loggerMock;
        private IDataService dataService;
        private readonly Appsettings appSetting = new Appsettings { EndPoint = "test" };
        private readonly Mock<IOptions<Appsettings>> mock = new Mock<IOptions<Appsettings>>();

        public DataServiceTest()
        {
            loggerMock = new Mock<ILogger<DataService>>();
            mock.Setup(ap => ap.Value).Returns(appSetting);
        }

        [TestMethod]
        public async Task GetData_ShouldReturnResponseAsync()
        {
            // Arrange
            const string testContent = @"[{""name"":""Bob"",""gender"":""Male"",""age"":23, ""pets"":
                                            [{ ""name"":""Garfield"",
                                                 ""type"":""Cat""}]}]";

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(testContent)
                });

            var httpClient = new HttpClient(mockMessageHandler.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            dataService = new DataService(loggerMock.Object, mock.Object.Value, httpClient);


            // Act
            var result = await dataService.GetDataAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(Response));
            Assert.AreEqual(result.Error, string.Empty);
            Assert.AreEqual(result.OwnerList.Count, 1);
            Assert.AreEqual(result.OwnerList[0].Pets.Count, 1);
            Assert.AreEqual(result.OwnerList[0].Pets[0].Name, "Garfield");
        }

        [TestMethod]
        public async Task GetData_ShouldReturnErrorAsync()
        {
            // Arrange
            const string testContent = @"[]";

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent(testContent)
                });

            var httpClient = new HttpClient(mockMessageHandler.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            dataService = new DataService(loggerMock.Object, mock.Object.Value, httpClient);


            // Act
            var result = await dataService.GetDataAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(Response));
            Assert.AreEqual(result.Error, "Service not reachable:error code NotFound");
            Assert.AreEqual(result.OwnerList.Count, 0);
        }
    }
}
