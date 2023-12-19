namespace API.Analyzer.Domain.DTOs

{
    public class GetWalletResponseDTO
    {
        public string Id { get; set; }
        public decimal Balance { get; set; }
        public bool IsDemo { get; set; }
        public string UserName { get; set; }
    }
}

        