using API.Accounts.Domain.Interfaces;

namespace API.Accounts.Domain.Entities
{
    public class User : IEntity
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
