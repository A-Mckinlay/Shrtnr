using System.Net.Http;
using System.Threading.Tasks;

namespace UrlValidationClient
{
    public class UrlValidator : IUrlValidator
    {
        private readonly HttpClient _httpClient;

        public UrlValidator(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> ValidateUrl(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            return await _httpClient.SendAsync(request);
        }
    }
}
