using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
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
			//Arrange
			_httpClientMock.Setup(x => x.GetStringAsync(It.IsAny<string>())).ReturnsAsync("5000");
			var buyStockDTO = new BuyStockDTO { UserId = "1", StockId = "1", TotalBuyingPriceWithoutCommission = 1000m };
			var expectedUpdatedAccountBalance = 5000 - (buyStockDTO.TotalBuyingPriceWithoutCommission * 1.0005m);
			var expectedBuyStockResponseDTO = new BuyStockResponseDTO { IsSuccessful = true, Message = "Transaction accepted!", UpdatedAccountBalance = expectedUpdatedAccountBalance };

			//Act
			var actualBuyStockResponseDTO = await _settlementService.BuyStock(buyStockDTO);

			//Assert
			Assert.AreEqual(expectedBuyStockResponseDTO.IsSuccessful, actualBuyStockResponseDTO.IsSuccessful);
			Assert.AreEqual(expectedBuyStockResponseDTO.Message, actualBuyStockResponseDTO.Message);
			Assert.AreEqual(expectedBuyStockResponseDTO.UpdatedAccountBalance, actualBuyStockResponseDTO.UpdatedAccountBalance);
		}

		[Test]
		public async Task BuyStock_ExactBalance_ReturnsSuccess()
		{
			//Arrange
			_httpClientMock.Setup(x => x.GetStringAsync(It.IsAny<string>())).ReturnsAsync("1000.5");
			var buyStockDTO = new BuyStockDTO { UserId = "1", StockId = "1", TotalBuyingPriceWithoutCommission = 1000m };
			var expectedUpdatedAccountBalance = 1000.5m - (buyStockDTO.TotalBuyingPriceWithoutCommission * 1.0005m);
			var expectedBuyStockResponseDTO = new BuyStockResponseDTO { IsSuccessful = true, Message = "Transaction accepted!", UpdatedAccountBalance = expectedUpdatedAccountBalance };

			//Act
			var actualBuyStockResponseDTO = await _settlementService.BuyStock(buyStockDTO);

			//Assert
			Assert.AreEqual(expectedBuyStockResponseDTO.IsSuccessful, actualBuyStockResponseDTO.IsSuccessful);
			Assert.AreEqual(expectedBuyStockResponseDTO.Message, actualBuyStockResponseDTO.Message);
			Assert.AreEqual(expectedBuyStockResponseDTO.UpdatedAccountBalance, actualBuyStockResponseDTO.UpdatedAccountBalance);
		}

		[Test]
		public async Task BuyStock_NotEnoughBalance_ReturnsFail()
		{
			//Arrange
			_httpClientMock.Setup(x => x.GetStringAsync(It.IsAny<string>())).ReturnsAsync("1");
			var buyStockDTO = new BuyStockDTO { UserId = "1", StockId = "1", TotalBuyingPriceWithoutCommission = 1000 };
			var expectedUpdatedAccountBalance = 1;
			var expectedBuyStockResponseDTO = new BuyStockResponseDTO { IsSuccessful = false, Message = "Transaction declined!", UpdatedAccountBalance = expectedUpdatedAccountBalance };

			//Act
			var actualBuyStockResponseDTO = await _settlementService.BuyStock(buyStockDTO);

			//Assert
			Assert.AreEqual(expectedBuyStockResponseDTO.IsSuccessful, actualBuyStockResponseDTO.IsSuccessful);
			Assert.AreEqual(expectedBuyStockResponseDTO.Message, actualBuyStockResponseDTO.Message);
			Assert.AreEqual(expectedBuyStockResponseDTO.UpdatedAccountBalance, actualBuyStockResponseDTO.UpdatedAccountBalance);
		}

		[Test]
		public async Task SellStock_ReturnsSuccess()
		{
			//Arrange
			_httpClientMock.Setup(x => x.GetStringAsync(It.IsAny<string>())).ReturnsAsync("0");
			var sellStockDTO = new SellStockDTO { UserId = "1", StockId = "1", TotalSellingPriceWithoutCommission = 1000m };
			decimal expectedUpdatedAccountBalance = 0 + (sellStockDTO.TotalSellingPriceWithoutCommission * 0.9995m);
			var expectedSellStockResponseDTO = new SellStockResponseDTO { IsSuccessful = true, Message = "Transaction accepted!", UpdatedAccountBalance = expectedUpdatedAccountBalance };

			//Act
			var actualSellStockResponseDTO = await _settlementService.SellStock(sellStockDTO);

			//Assert
			Assert.AreEqual(expectedSellStockResponseDTO.IsSuccessful, actualSellStockResponseDTO.IsSuccessful);
			Assert.AreEqual(expectedSellStockResponseDTO.Message, actualSellStockResponseDTO.Message);
			Assert.AreEqual(expectedSellStockResponseDTO.UpdatedAccountBalance, actualSellStockResponseDTO.UpdatedAccountBalance);
		}

	}
}