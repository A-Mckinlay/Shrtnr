using System.Threading.Tasks;
using ShrtnrTableClient.Model;

namespace ShrtnrTableClient.Repository
{
    public interface IShrtUrlRepo
    {
        Task<ShrtUrlEntity> AddShrtUrl(ShrtUrlEntity shrtUrlEntity);
    }
}
