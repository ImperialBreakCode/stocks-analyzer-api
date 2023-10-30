namespace API.Analyzer.Domain.DTOs
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public decimal Amount { get; set; }

        //  public ICollection<StockDTO>StockDTOs { get; set; }
        public User(string Name, decimal amount)
        {
            Username = Name;
            Amount = amount;
        }
    }
}