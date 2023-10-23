using API.Accounts.Domain.Interfaces;

namespace API.Accounts.Domain.Entities
{
    public class Wallet : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}
