using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UrlValidationClient;

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
        public async Task<string> PostAsync(string url)
        {
            var res = await _urlValidator.ValidateUrl(url);
            _logger.LogDebug(res.StatusCode.ToString());
            return "someshrturlforya";
        }
    }
}
