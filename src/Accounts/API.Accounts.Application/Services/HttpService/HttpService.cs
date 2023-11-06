﻿using Newtonsoft.Json;

namespace API.Accounts.Application.Services.HttpService
{
    public class HttpService : IHttpService
    {
        private HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T?> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string jsonData = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        public async Task PostAsync(string url, object data)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(data));
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}