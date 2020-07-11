using Moq;
using Xunit;
using System.Net.Http;
using RichardSzalay.MockHttp;
using System.Net;
using UrlValidationClient;

namespace UrlValidationClientTests
{
    public class UrlValidatorTests
    {
        [Fact]
        public async System.Threading.Tasks.Task ValidateUrlShouldContructTheCorrectHttpRequestAndSendIt()
        {
            var url = "http://example.com/api/doMagic";

            var expected = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK
            };

            var mockHttpMessageHandler = new MockHttpMessageHandler();
            mockHttpMessageHandler.When(url).Respond(req => expected);

            var mockHttpClient = mockHttpMessageHandler.ToHttpClient();

            var urlValidator = new UrlValidator(mockHttpClient);

            var actual = await urlValidator.ValidateUrl(url);

            Assert.Equal(HttpMethod.Get, actual.RequestMessage.Method);
            Assert.Equal(url, actual.RequestMessage.RequestUri.ToString());
            Assert.Equal(expected, actual);
        }
    }
}
