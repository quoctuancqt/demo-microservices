namespace Common.Proxy
{
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class ProxyBase
    {
        protected readonly HttpClient _httpClient;

        public ProxyBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public virtual async Task<HttpResponseMessage> PostAsJsonAsync(string endPoint, object obj)
        {
            return await _httpClient.PostAsync(endPoint, ObjToHttpContent(obj));
        }

        public virtual async Task<HttpResponseMessage> PostAsync(string endPoint, HttpContent content)
        {
            return await _httpClient.PostAsync(endPoint, content);
        }

        public virtual async Task<HttpResponseMessage> PutAsJsonAsync(string endPoint, object obj)
        {
            return await _httpClient.PutAsync(endPoint, ObjToHttpContent(obj));
        }

        public virtual async Task<HttpResponseMessage> GetAsync(string endPoint)
        {
            return await _httpClient.GetAsync(endPoint);
        }

        public virtual async Task<HttpResponseMessage> DeteleAsync(string endPoint)
        {
            return await _httpClient.DeleteAsync(endPoint);
        }

        public virtual void SetToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private StringContent ObjToHttpContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
    }
}
