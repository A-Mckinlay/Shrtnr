using System.Threading.Tasks;
using ShrtnrTableClient.Model;
using ShrtnrTableClient.Model.Dto;

namespace ShrtnrTableClient.Repository
{
    public interface IShrtUrlRepo
    {
        Task<ShrtUrlEntity> AddShrtUrl(UrlHashPair urlHashPair);
    }
}
