﻿namespace API.Settlement.Domain.DTOs.Response.FinalizeDTOs
{
	public class StockInfoResponseDTO
	{
		public string TransactionId { get; set; }
		public string Message { get; set; }
		public string StockId { get; set; }
		public string StockName { get; set; }
		public int Quantity { get; set; }
		public decimal SinglePriceIncludingCommission { get; set; }
		public decimal TotalPriceIncludingCommission => Quantity * SinglePriceIncludingCommission;
	}
}