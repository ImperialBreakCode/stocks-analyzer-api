namespace API.Accounts.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public Wallet()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public decimal Balance { get; set; }
        public bool IsDemo { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
    }
}
