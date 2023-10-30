using API.Accounts.Domain.Interfaces;

namespace API.Accounts.Domain.Entities
{
    public abstract class BaseEntity : IEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
    }
}
