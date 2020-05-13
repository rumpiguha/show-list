using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using ShowCats.Models;
using ShowCats.Services;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ShowPetsTest
{
    [TestClass]
    public class DataServiceTest
    {
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

            var configMock = new Mock<IConfigurationRoot>();
            configMock.SetupGet(x => x[It.IsAny<string>()]).Returns("http://test/people.json");

            var dataService = new DataService(new HttpClient(mockMessageHandler.Object), configMock.Object);

            // Act
            var result = await dataService.GetData();

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

            var configMock = new Mock<IConfigurationRoot>();
            configMock.SetupGet(x => x[It.IsAny<string>()]).Returns("http://test/people.json");

            var dataService = new DataService(new HttpClient(mockMessageHandler.Object), configMock.Object);

            // Act
            var result = await dataService.GetData();

            // Assert
            Assert.IsInstanceOfType(result, typeof(Response));
            Assert.AreEqual(result.Error, "Service not reachable:error code NotFound");
            Assert.AreEqual(result.OwnerList.Count, 0);
        }
    }
}
