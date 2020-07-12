using System;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UrlValidationClient;
using ShrtnrTableClient.Repository;
using ShrtnrTableClient.Model.Dto;
using Shrtnr.HashIds;

namespace Shrtnr.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShrtnController : ControllerBase
    {
        private readonly ILogger<ShrtnController> _logger;
        private readonly IUrlValidator _urlValidator;
        private readonly IShrtUrlRepo _shrtUrlRepo;
        private readonly IHasher _hasher;

        public ShrtnController(
            ILogger<ShrtnController> logger,
            IUrlValidator urlValidator,
            IShrtUrlRepo shrtUrlRepo,
            IHasher hasher
        ) {
            _logger = logger;
            _urlValidator = urlValidator;
            _shrtUrlRepo = shrtUrlRepo;
            _hasher = hasher;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] string url)
        {
            if (url == null) return new BadRequestResult();

            try
            {
                var res = await _urlValidator.ValidateUrl(url);

                if (res.IsSuccessStatusCode)
                {
                    var result = await _shrtUrlRepo.AddShrtUrl(new ShrtUrlDto
                    {
                        Url = url,
                        Code = _hasher.GetCode()
                    });

                    return new OkObjectResult(result);
                }

                dynamic invalidUrlResult = new ExpandoObject();
                invalidUrlResult.message = "Sorry shrtnr could not verify your URL works.";

                return new NotFoundObjectResult(invalidUrlResult);
            }
            catch (Exception ex)
            {
                _logger.LogError("Endpoint threw Exception: ", ex);
                return new StatusCodeResult(500);
            }
        }
    }
}
