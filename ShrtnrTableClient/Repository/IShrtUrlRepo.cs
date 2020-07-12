using System.Threading.Tasks;
using ShrtnrTableClient.Model;
using ShrtnrTableClient.Model.Dto;

namespace ShrtnrTableClient.Repository
{
    public interface IShrtUrlRepo
    {
        Task<ShrtUrlDto> AddShrtUrl(ShrtUrlDto shrtUrlDto);
        Task<ShrtUrlEntity> LookupShrtUrl(string code);
        Task<ShrtUrlDto> IncrementClicks(ShrtUrlEntity shrtUrlDto);
    }
}
