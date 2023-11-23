namespace API.Accounts.Application.Services.UserService.EmailService
{
    public interface IEmailConfirmation
    {
        bool SendEmail(string email, string userId);
    }
}
