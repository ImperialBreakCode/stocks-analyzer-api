namespace API.Accounts.Application.DTOs.SharedDTOs
{
    public class FinalizeStockActionDTO
    {
        public string WalletId { get; set; }
        public string StockId { get; set; }
        public int Quantity { get; set; }
        public decimal SinglePrice { get; set; }
    }
}
