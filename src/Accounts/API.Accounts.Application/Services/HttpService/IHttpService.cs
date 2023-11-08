namespace API.Accounts.Application.Services.HttpService
{
    public interface IHttpService : IDisposable
    {
        public Task<T?> GetAsync<T>(string url);
        public Task<T?> PostAsync<T>(string url, object data);
    }
}
