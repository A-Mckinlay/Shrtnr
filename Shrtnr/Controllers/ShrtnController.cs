using System;
using System.Dynamic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UrlValidationClient;
using HashidsNet;

namespace Shrtnr.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShrtnController : ControllerBase
    {
        private readonly ILogger<ShrtnController> _logger;
        private readonly IUrlValidator _urlValidator;

        public ShrtnController(ILogger<ShrtnController> logger, IUrlValidator urlValidator)
        {
            _logger = logger;
            _urlValidator = urlValidator;
        }

        [HttpPost]
        public async Task<ActionResult<string>> PostAsync(string url)
        {
            if (url == null) return new BadRequestResult();

            try
            {
                var res = await _urlValidator.ValidateUrl(url);

                if (res.IsSuccessStatusCode)
                {
                    var hashids = new Hashids("this is my salt", 6);
                    int timestamp = (int)(Int64)(DateTime.UtcNow.Subtract(DateTime.UnixEpoch)).TotalMilliseconds;
                    var hash = hashids.Encode(Math.Abs(timestamp));
                    dynamic urlHashPair = new ExpandoObject();
                    urlHashPair.url = url;
                    urlHashPair.hash = hash;
                    return new OkObjectResult(urlHashPair);
                }

                dynamic invalidUrlResult = new ExpandoObject();
                invalidUrlResult.message = "Sorry shrtnr could not verify your URL works.";

                return new NotFoundObjectResult(invalidUrlResult);
            }
            catch (Exception ex)
            {
                _logger.LogError("Url Validation Threw Exception: ", ex);
                return new StatusCodeResult(500);
            }
        }
    }
}
