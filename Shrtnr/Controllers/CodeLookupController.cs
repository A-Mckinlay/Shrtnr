using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShrtnrTableClient.Repository;

namespace Shrtnr.Controllers
{
    [ApiController]
    [Route("/{code:minlength(6):regex(^[[a-zA-Z0-9]]*$)}")]
    public class CodeLookupController
    {

        private readonly ILogger<ShrtnController> _logger;
        private readonly IShrtUrlRepo _shrtUrlRepo;

        public CodeLookupController(
            ILogger<ShrtnController> logger,
            IShrtUrlRepo shrtUrlRepo
        )
        {
            _logger = logger;
            _shrtUrlRepo = shrtUrlRepo;
        }

        [HttpGet]
        public async Task<ActionResult> LookupCode(string code)
        {
            var shrtUrlEntity = await _shrtUrlRepo.LookupShrtUrl(code);

            if (shrtUrlEntity == null) return new NotFoundResult();

            await _shrtUrlRepo.IncrementClicks(shrtUrlEntity);

            return new RedirectResult(shrtUrlEntity.Url);
        }

    }
}
