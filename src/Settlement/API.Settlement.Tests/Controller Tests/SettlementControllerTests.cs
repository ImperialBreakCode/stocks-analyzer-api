using API.Settlement.Controllers;
using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;
using API.Settlement.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Tests.Controller_Tests
{
	[TestFixture]
	public class SettlementControllerTests
	{
		private Mock<ISettlementService> _settlementServiceMock;
		private SettlementController _settlementController;
		private Mock<IHttpClient> _httpClientMock;

		[SetUp]
		public void Setup()
		{
			_settlementServiceMock = new Mock<ISettlementService>();
			_settlementController = new SettlementController(_settlementServiceMock.Object);
		}

		[Test]
		public async Task BuyStock_ReturnsOk_WhenSuccessful()
		{
			//Arrange
			var buyStockDTO = new BuyStockDTO { UserId = "1", StockId = "1", TotalBuyingPriceWithoutCommission = 1000m };
			var expectedBuyStockResponseDTO = new BuyStockResponseDTO { IsSuccessful = true, Message = "Transaction accepted!", UpdatedAccountBalance = 0m };
			_settlementServiceMock.Setup(x => x.BuyStock(It.IsAny<BuyStockDTO>())).ReturnsAsync(expectedBuyStockResponseDTO);

			//Act
			var actualIActionResult = await _settlementController.BuyStock(buyStockDTO);
			var actualOkObjectResult = actualIActionResult as OkObjectResult;
			var actualBuyStockResponseDTO = actualOkObjectResult.Value as BuyStockResponseDTO;

			//Assert
			Assert.IsNotNull(actualIActionResult);
			Assert.IsInstanceOf<OkObjectResult>(actualIActionResult);

			Assert.AreEqual(200, actualOkObjectResult.StatusCode);
			Assert.AreEqual(expectedBuyStockResponseDTO.IsSuccessful, actualBuyStockResponseDTO.IsSuccessful);
			Assert.AreEqual(expectedBuyStockResponseDTO.Message, actualBuyStockResponseDTO.Message);
			Assert.AreEqual(expectedBuyStockResponseDTO.UpdatedAccountBalance, actualBuyStockResponseDTO.UpdatedAccountBalance);
		}

		[Test]
		public async Task SellStock_ReturnsOk_WhenSuccessful()
		{
			//Arrange
			var sellStockDTO = new SellStockDTO { UserId = "1", StockId = "1", TotalSellingPriceWithoutCommission = 1000 };
			var expectedSellStockResponseDTO = new SellStockResponseDTO { IsSuccessful = true, Message = "Transaction accepted!", UpdatedAccountBalance = 999.5m };
			_settlementServiceMock.Setup(x => x.SellStock(It.IsAny<SellStockDTO>())).ReturnsAsync(expectedSellStockResponseDTO);

			//Act
			var actualIActionResult = await _settlementController.SellStock(sellStockDTO);
			var actualOkObjectResult = actualIActionResult as OkObjectResult;
			var actualSellStockResponseDTO = actualOkObjectResult.Value as SellStockResponseDTO;

			//Assert
			Assert.IsNotNull(actualIActionResult);
			Assert.IsInstanceOf<OkObjectResult>(actualIActionResult);

			Assert.AreEqual(200, actualOkObjectResult.StatusCode);
			Assert.AreEqual(expectedSellStockResponseDTO.IsSuccessful, actualSellStockResponseDTO.IsSuccessful);
			Assert.AreEqual(expectedSellStockResponseDTO.Message, actualSellStockResponseDTO.Message);
			Assert.AreEqual(expectedSellStockResponseDTO.UpdatedAccountBalance, actualSellStockResponseDTO.UpdatedAccountBalance);

		}
	}
}