using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using TopView.Model.Models;
using TopView.Services;
using Xunit;

namespace TopView.Tests.Services
{
	public class StockServiceTests
	{
		[Fact]
		public async Task GetQuoteAsync_ReturnsQuote_WhenApiReturnsSuccess()
		{
			// Arrange
 var expected = new Quote (123.45m,1.4m,0.1m, 123m, 112313132);
			var handlerMock = new Mock<HttpMessageHandler>();
			handlerMock.Protected()
			.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
			.ReturnsAsync(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = JsonContent.Create(expected)
			});
			var httpClient = new HttpClient(handlerMock.Object);
			var service = new StockService("dummy-token", httpClient);

			// Act
			var result = await service.GetQuoteAsync("AAPL");

			// Assert
			Assert.NotNull(result);
			Assert.Equal(expected.c, result.c);
		}

		[Fact]
		public async Task GetQuoteAsync_ReturnsNull_OnException()
		{
			// Arrange
			var handlerMock = new Mock<HttpMessageHandler>();
			handlerMock.Protected()
			.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
			.ThrowsAsync(new HttpRequestException("Network error"));
			var httpClient = new HttpClient(handlerMock.Object);
			var service = new StockService("dummy-token", httpClient);

			// Act
			var result = await service.GetQuoteAsync("AAPL");

			// Assert
			Assert.Null(result);
		}

		[Fact]
		public async Task GetQuoteAsync_ThrowsArgumentException_OnEmptySymbol()
		{
			// Arrange
			var service = new StockService("dummy-token");

			// Act & Assert
			await Assert.ThrowsAsync<ArgumentException>(() => service.GetQuoteAsync(""));
		}
	}
}
