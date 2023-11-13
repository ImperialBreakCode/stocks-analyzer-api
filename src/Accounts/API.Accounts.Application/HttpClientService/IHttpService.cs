namespace API.Accounts.Application.HttpClientService
{
    public interface IHttpService : IDisposable
    {
        public Task<T?> GetAsync<T>(string url);
        public Task<T?> PostAsync<T>(string url, object data);
    }
}
