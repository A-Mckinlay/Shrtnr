using System.Net.Http;
using System.Threading.Tasks;

namespace UrlValidationClient
{
    public interface IUrlValidator
    {
        public Task<HttpResponseMessage> ValidateUrl(string url);
    }
}
