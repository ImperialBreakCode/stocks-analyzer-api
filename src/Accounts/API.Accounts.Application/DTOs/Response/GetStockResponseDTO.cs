namespace API.Accounts.Application.DTOs.Response
{
    public class GetStockResponseDTO
    {
        public string StockId { get; set; }
        public string StockName { get; set; }
        public int Quantity { get; set; }
        public string WalletId { get; set; }
    }
}
