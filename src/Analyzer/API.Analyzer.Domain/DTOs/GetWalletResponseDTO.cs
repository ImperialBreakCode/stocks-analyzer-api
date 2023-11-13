namespace API.Analyzer.Domain.DTOs

{
    public class GetWalletResponseDTO
    {
        public string Id { get; set; }
        public decimal Balance { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
    }
}

        