using API.Accounts.Domain.Interfaces;

namespace API.Accounts.Domain.Entities
{
    public class Wallet : IEntity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
    }
}
