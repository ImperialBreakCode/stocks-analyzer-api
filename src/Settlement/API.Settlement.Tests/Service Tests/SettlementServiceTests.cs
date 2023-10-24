using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.Interfaces;
using API.Settlement.DTOs.Request;
using API.Settlement.Infrastructure.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Tests
{
	[TestFixture]
	public class SettlementServiceTests
	{
		private Mock<IHttpClient> _httpClientMock;
		private SettlementService _settlementService;

		[SetUp]
		public void SetUp()
		{
			_httpClientMock = new Mock<IHttpClient>();
			_settlementService = new SettlementService(_httpClientMock.Object);

		}
		[Test]
		public async Task BuyStock_EnoughBalance_ReturnsSuccess()
		{
			_httpClientMock.Setup(x => x.GetStringAsync(It.IsAny<string>())).ReturnsAsync("5000");
			var buyStockDTO = new BuyStockDTO { UserId = "1", StockId = "1", TotalBuyingPriceWithoutCommission = 1000m };

			var result = await _settlementService.BuyStock(buyStockDTO);

			Assert.IsTrue(result.IsSuccessful);
			Assert.AreEqual("Transaction accepted!", result.Message);
		}
		[Test]
		public async Task BuyStock_ExactBalance_ReturnsSuccess()
		{
			_httpClientMock.Setup(x => x.GetStringAsync(It.IsAny<string>())).ReturnsAsync("1050");
			var buyStockDTO = new BuyStockDTO { UserId = "1", StockId = "1", TotalBuyingPriceWithoutCommission = 1000m };

			var result = await _settlementService.BuyStock(buyStockDTO);

			Assert.IsTrue(result.IsSuccessful);
			Assert.AreEqual("Transaction accepted!", result.Message);
		}

		[Test]
		public async Task BuyStock_NotEnoughBalance_ReturnsFail()
		{
			_httpClientMock.Setup(x => x.GetStringAsync(It.IsAny<string>())).ReturnsAsync("1000");
			var buyStockDTO = new BuyStockDTO { UserId = "1", StockId = "1", TotalBuyingPriceWithoutCommission = 1000m };

			var result = await _settlementService.BuyStock(buyStockDTO);

			Assert.IsFalse(result.IsSuccessful);
			Assert.AreEqual("Transaction declined!", result.Message);
		}

		[Test]
		public async Task SellStock_ReturnsSuccess()
		{
			_httpClientMock.Setup(x => x.GetStringAsync(It.IsAny<string>())).ReturnsAsync("0");
			var sellStockDTO = new SellStockDTO { UserId = "1", StockId = "1", TotalSellingPriceWithoutCommission = 1000m };
			decimal currentBalance = 0;
			decimal expectedUpdatedBalance = currentBalance + (sellStockDTO.TotalSellingPriceWithoutCommission * 0.95m);

			var result = await _settlementService.SellStock(sellStockDTO);

			Assert.IsTrue(result.IsSuccessful);
			Assert.AreEqual("Transaction accepted!", result.Message);
			Assert.AreEqual(expectedUpdatedBalance, result.UpdatedAccountBalance);
		}
	}
}
