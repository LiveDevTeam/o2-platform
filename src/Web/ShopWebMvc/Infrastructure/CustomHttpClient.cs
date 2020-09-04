using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ShopWebMvc.Infrastructure
{
    public class CustomHttpClient : IHttpClient
    {
        private HttpClient _client;
        private ILogger<CustomHttpClient> _logger;

        public CustomHttpClient(ILogger<CustomHttpClient> logger)
        {
            _client = new HttpClient();
            _logger = logger;
        }
        public async Task<string> GetStringAsync(string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _client.SendAsync(requestMessage);
            return await response.Content.ReadAsStringAsync();
        }


        private async Task<HttpResponseMessage> DoPostPutAsync<T>(HttpMethod method, string url, T item)
        {
            if(method!= HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put.", nameof(method));
            }

            //a new StringContent must be created for each retry
            //as it is desposed after each call

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(requestMessage);

            if(response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException();
            }
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string url, T item)
        {
            return await DoPostPutAsync(HttpMethod.Post, url, item);
        }

        public async Task<HttpResponseMessage> DeleteAsync<T>(string url)
        {
            var requestMessage =new  HttpRequestMessage(HttpMethod.Delete, url);

            return await _client.SendAsync(requestMessage);
        }
      

        public async Task<HttpResponseMessage> PutAsync<T>(string url, T item)
        {
            return await DoPostPutAsync(HttpMethod.Put, url, item);
        }
    }
}
