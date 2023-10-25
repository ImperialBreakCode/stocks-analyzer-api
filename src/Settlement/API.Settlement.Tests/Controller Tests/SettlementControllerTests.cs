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

		[SetUp]
		public void Setup()
		{
			_settlementServiceMock = new Mock<ISettlementService>();
			_settlementController = new SettlementController(_settlementServiceMock.Object);
		}

		//[Test]
		//public async Task BuyStock_ReturnsOk_WhenSuccessful()
		//{
		//	//Arrange
		//	var buyStockDTO = new BuyStockDTO { UserId = "1", StockId = "1", TotalBuyingPriceWithoutCommission = 1000m };
		//	var expectedUpdatedAccountBalance = 4000.5m;
		//	var expectedBuyStockResponseDTO = new BuyStockResponseDTO { IsSuccessful = true, Message = "Transaction accepted!", UpdatedAccountBalance = expectedUpdatedAccountBalance };
		//	_settlementServiceMock.Setup(x => x.BuyStock(It.IsAny<BuyStockDTO>())).ReturnsAsync(expectedBuyStockResponseDTO);

		//	//Act
		//	var actualActionResult = await _settlementController.BuyStock(buyStockDTO);

		//	//Assert
		//	Assert.IsNotNull(actualActionResult);
		//	Assert.IsInstanceOf<OkObjectResult>(actualActionResult);
		//	var okObjectResult = actualActionResult as OkObjectResult;
		//	Assert.AreEqual(200, okObjectResult.StatusCode);
		//	Assert.AreEqual(expectedBuyStockResponseDTO, okObjectResult.Value);
		//}

		//[Test]
		//public async Task SellStock_ReturnsOk_WhenSuccessful()
		//{
		//	var sellStockDTO = new SellStockDTO { UserId = "1", StockId = "1", TotalSellingPriceWithoutCommission = 1000m };
		//	var responseDTO = new SellStockResponseDTO { IsSuccessful = true, Message = "Transaction accepted!", UpdatedAccountBalance = 999.5m };
		//	_settlementServiceMock.Setup(x => x.SellStock(It.IsAny<SellStockDTO>())).ReturnsAsync(responseDTO);

		//	var result = await _settlementController.SellStock(sellStockDTO);

		//	Assert.IsNotNull(result);
		//	Assert.IsInstanceOf<OkObjectResult>(result);
		//	var okResult = result as OkObjectResult;
		//	Assert.AreEqual(200, okResult.StatusCode);
		//	Assert.AreEqual(responseDTO, okResult.Value);
		//}
	}
}