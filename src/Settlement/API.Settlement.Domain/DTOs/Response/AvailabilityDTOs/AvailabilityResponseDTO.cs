﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.DTOs.Response.AvailabilityDTOs
{
    public class AvailabilityResponseDTO
    {
		public string WalletId { get; set; }
		public string UserId { get; set; }
		public bool IsSale { get; set; }
		public IEnumerable<AvailabilityStockInfoResponseDTO> AvailabilityStockInfoResponseDTOs { get; set; }
		public decimal TotalSuccessfulPrice => AvailabilityStockInfoResponseDTOs.Where(ь => ь.IsSuccessful).Sum(ь => ь.TotalPriceIncludingCommission);
	}
}