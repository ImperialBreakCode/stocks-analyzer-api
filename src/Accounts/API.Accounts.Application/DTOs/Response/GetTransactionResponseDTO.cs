namespace API.Accounts.Application.DTOs.Response
{
    public class GetTransactionResponseDTO
    {
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string StockId { get; set; }
        public string Walletid { get; set; }
    }
}
