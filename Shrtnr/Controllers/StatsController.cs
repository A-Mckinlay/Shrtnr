using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShrtnrTableClient.Model.Dto;
using ShrtnrTableClient.Repository;

namespace Shrtnr.Controllers
{
    [ApiController]
    [Route("stats")]
    public class StatsController : ControllerBase
    {
        private readonly IShrtUrlRepo _shrtnUrlRepo;

        public StatsController(IShrtUrlRepo shrtUrlRepo)
        {
            _shrtnUrlRepo = shrtUrlRepo;
        }

        [HttpGet("{code}")]
        public async Task<ActionResult> GetStats(string code)
        {
            if (code == null) return new BadRequestResult();

            var shrtUrlEntity = await _shrtnUrlRepo.LookupShrtUrl(code);

            if (shrtUrlEntity == null) return new NotFoundResult();

            return new OkObjectResult(new ShrtUrlDto().FromShrtUrlEntity(shrtUrlEntity));
        }
    }
}
