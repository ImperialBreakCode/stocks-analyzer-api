using API.Settlement.Domain.Enums;
using System.Text.Json.Serialization;

namespace API.Settlement.Domain.DTOs.Request
{
	public class FinalizeTransactionRequestDTO
	{
		public string WalletId { get; set; }
		public string UserId { get; set; }
		public string UserEmail { get; set; }
		public bool IsSale { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public UserType UserRank { get; set; }
		public IEnumerable<StockInfoRequestDTO> StockInfoRequestDTOs { get; set; }
	}
}