using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShopWebMvc.Infrastructure
{
    public interface IHttpClient
    {
        Task<string> GetStringAsync(string url);
        Task<HttpResponseMessage> PostAsync<T>(string url, T item);
        Task<HttpResponseMessage> DeleteAsync<T>(string url);
        Task<HttpResponseMessage> PutAsync<T>(string url, T item);
    }
}
