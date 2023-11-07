namespace API.Accounts.Application.Services.HttpService
{
    public interface IHttpService : IDisposable
    {
        public Task<T?> GetAsync<T>(string url);
        public Task PostAsync(string url, object data);
    }
}
