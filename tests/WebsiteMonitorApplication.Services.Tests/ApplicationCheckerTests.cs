using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Moq;
using WebsiteMonitorApplication.Core;
using WebsiteMonitorApplication.Core.Entities;
using WebsiteMonitorApplication.Core.Enums;
using WebsiteMonitorApplication.Core.Services;

namespace WebsiteMonitorApplication.Services.Tests
{
    public class ApplicationCheckerTests
    {
        [Fact]
        public async Task CheckApplicationAsync_HttpRequestException_DoesNotThrowException()
        {
            // arrange
            var httpClientMock = new Mock<IHttpClient>();
            httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ThrowsAsync(new HttpRequestException());

            var sut = CreateSut(httpClientMock.Object);

            CheckResult result = null;
            // act
            result = await sut.CheckApplicationAsync(new Application { Url = "http://localhost" });

            // assert
            Assert.NotNull(result);
            Assert.Equal(ApplicationState.CheckedWithError, result.State);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CheckApplicationAsync_Exception_DoesNotThrowException()
        {
            // arrange
            var httpClientMock = new Mock<IHttpClient>();
            httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ThrowsAsync(new Exception());

            var sut = CreateSut(httpClientMock.Object);

            CheckResult result = null;
            // act
            result = await sut.CheckApplicationAsync(new Application { Url = "http://localhost" });

            // assert
            Assert.NotNull(result);
            Assert.Equal(ApplicationState.CheckedWithError, result.State);
            Assert.False(result.IsSuccess);
        }

        [Theory]
        [InlineData(HttpStatusCode.Continue)]
        [InlineData(HttpStatusCode.SwitchingProtocols)]
        public async Task CheckApplicationAsync_HttpCode_100s_Success(HttpStatusCode statusCode)
        {
            // arrange
            var httpClientMock = new Mock<IHttpClient>();
            httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage(statusCode));

            var sut = CreateSut(httpClientMock.Object);

            CheckResult result = null;
            // act
            result = await sut.CheckApplicationAsync(new Application { Url = "http://localhost" });

            // assert
            Assert.NotNull(result);
            Assert.Equal(ApplicationState.Available, result.State);
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.Created)]
        [InlineData(HttpStatusCode.Accepted)]
        [InlineData(HttpStatusCode.NonAuthoritativeInformation)]
        [InlineData(HttpStatusCode.NoContent)]
        [InlineData(HttpStatusCode.ResetContent)]
        [InlineData(HttpStatusCode.PartialContent)]
        public async Task CheckApplicationAsync_HttpCode_200s_Success(HttpStatusCode statusCode)
        {
            // arrange
            var httpClientMock = new Mock<IHttpClient>();
            httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage(statusCode));

            var sut = CreateSut(httpClientMock.Object);

            CheckResult result = null;
            // act
            result = await sut.CheckApplicationAsync(new Application { Url = "http://localhost" });

            // assert
            Assert.NotNull(result);
            Assert.Equal(ApplicationState.Available, result.State);
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(HttpStatusCode.Ambiguous)]
        [InlineData(HttpStatusCode.Moved)]
        [InlineData(HttpStatusCode.Redirect)]
        [InlineData(HttpStatusCode.RedirectMethod)]
        [InlineData(HttpStatusCode.NotModified)]
        [InlineData(HttpStatusCode.UseProxy)]
        [InlineData(HttpStatusCode.Unused)]
        [InlineData(HttpStatusCode.TemporaryRedirect)]
        public async Task CheckApplicationAsync_HttpCode_300s_Success(HttpStatusCode statusCode)
        {
            // arrange
            var httpClientMock = new Mock<IHttpClient>();
            httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage(statusCode));

            var sut = CreateSut(httpClientMock.Object);

            CheckResult result = null;
            // act
            result = await sut.CheckApplicationAsync(new Application { Url = "http://localhost" });

            // assert
            Assert.NotNull(result);
            Assert.Equal(ApplicationState.Available, result.State);
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.PaymentRequired)]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.Conflict)]
        [InlineData(HttpStatusCode.ExpectationFailed)]
        [InlineData(HttpStatusCode.UpgradeRequired)]
        public async Task CheckApplicationAsync_HttpCode_400s_Error(HttpStatusCode statusCode)
        {
            // arrange
            var httpClientMock = new Mock<IHttpClient>();
            httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage(statusCode));

            var sut = CreateSut(httpClientMock.Object);

            CheckResult result = null;
            // act
            result = await sut.CheckApplicationAsync(new Application { Url = "http://localhost" });

            // assert
            Assert.NotNull(result);
            Assert.Equal(ApplicationState.Unavailable, result.State);
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.NotImplemented)]
        [InlineData(HttpStatusCode.BadGateway)]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        [InlineData(HttpStatusCode.GatewayTimeout)]
        [InlineData(HttpStatusCode.HttpVersionNotSupported)]
        public async Task CheckApplicationAsync_HttpCode_500s_Error(HttpStatusCode statusCode)
        {
            // arrange
            var httpClientMock = new Mock<IHttpClient>();
            httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage(statusCode));

            var sut = CreateSut(httpClientMock.Object);

            CheckResult result = null;
            // act
            result = await sut.CheckApplicationAsync(new Application { Url = "http://localhost" });

            // assert
            Assert.NotNull(result);
            Assert.Equal(ApplicationState.Unavailable, result.State);
            Assert.True(result.IsSuccess);
        }

        private ApplicationChecker CreateSut(IHttpClient client)
        {
            return new ApplicationChecker(client);
        }
    }
}