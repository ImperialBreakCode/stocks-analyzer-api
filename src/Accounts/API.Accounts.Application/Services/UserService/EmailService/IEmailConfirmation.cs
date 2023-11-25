namespace API.Accounts.Application.Services.UserService.EmailService
{
    public interface IEmailConfirmation
    {
        void SendEmail(string email, string userId);
    }
}
