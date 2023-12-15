using API.Gateway.Domain.Entities.SQLiteEntities;

namespace API.Gateway.Domain.Interfaces.Services
{
    public interface IEmailService
    {
        Task Create(Email email);
        bool Exists(string email);
        Task Delete(string email);
    }
}