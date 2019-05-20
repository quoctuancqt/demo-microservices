namespace Resilience
{
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Polly;
    using Polly.Wrap;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class ResilientHttpClient : IHttpClient
    {
        private readonly HttpClient _client;
        private AsyncPolicyWrap _policyWrapper;
        private ILogger<ResilientHttpClient> _logger;
        public HttpClient Inst => _client;

        public ResilientHttpClient(IAsyncPolicy[] policies, ILogger<ResilientHttpClient> logger)
        {
            _client = new HttpClient();
            _logger = logger;

            // Add Policies to be applied
            _policyWrapper = Policy.WrapAsync(policies);
        }

        public Task<string> GetStringAsync(string uri) =>
            HttpInvoker(() =>
                _client.GetStringAsync(uri));

        public Task<HttpResponseMessage> PostAsync<T>(string uri, T item) =>
            // a new StringContent must be created for each retry 
            // as it is disposed after each call
            HttpInvoker(() =>
            {
                var response = _client.PostAsync(uri, new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json"));
                // raise exception if HttpResponseCode 500 
                // needed for circuit breaker to track fails
                if (response.Result.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new HttpRequestException();
                }

                return response;
            });

        public Task<HttpResponseMessage> DeleteAsync(string uri) =>
            HttpInvoker(() => _client.DeleteAsync(uri));


        private Task<T> HttpInvoker<T>(Func<Task<T>> action) =>
            // Executes the action applying all 
            // the policies defined in the wrapper
            _policyWrapper.ExecuteAsync(() => action());
    }
}
