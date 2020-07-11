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
                    var sliceTimestamp = int.Parse(timestamp.ToString().Substring(2));
                    _logger.LogInformation($"LOOOK HERER I IS TIMESTAMP: {sliceTimestamp.ToString()}");
                    var hash = hashids.Encode(Math.Abs(timestamp));
                    dynamic urlHashPair = new ExpandoObject();
                    urlHashPair.url = url;
                    urlHashPair.hash = hash;
                    // Do the work to shorten the url
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

        // https://stackoverflow.com/questions/11454004/calculate-a-md5-hash-from-a-string
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
