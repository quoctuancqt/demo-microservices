using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Proxies
{
    public class GatewayApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GatewayApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync(string endPoint, object obj)
        {
            return await _httpClient.PostAsync(endPoint, ObjToHttpContent(obj));
        }

        public async Task<HttpResponseMessage> PostAsync(string endPoint, HttpContent content)
        {
            return await _httpClient.PostAsync(endPoint, content);
        }

        public async Task<HttpResponseMessage> PutAsJsonAsync(string endPoint, object obj)
        {
            return await _httpClient.PutAsync(endPoint, ObjToHttpContent(obj));
        }

        public async Task<HttpResponseMessage> GetAsync(string endPoint)
        {
            return await _httpClient.GetAsync(endPoint);
        }

        public async Task<HttpResponseMessage> DeteleAsync(string endPoint)
        {
            return await _httpClient.DeleteAsync(endPoint);
        }

        private StringContent ObjToHttpContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }

        public void SetToken()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

                if (!string.IsNullOrEmpty(token))
                {
                    var beareToken = token.Split("Bearer ")[1];

                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", beareToken);
                }
            }
        }
    }
}
