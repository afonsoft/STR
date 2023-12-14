using Eaf.Domain.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Eaf.Str.AWBs
{
    public interface IAwbManager : IDomainService
    {
        IQueryable<AwbAddress> AwbAddress { get; }
        IQueryable<AwbItem> AwbItems { get; }
        IQueryable<Awb> Awbs { get; }

        Task<Awb> CreateAwbAsync(Awb input);

        Task<Awb> UpdateAwbAsync(Awb input);

        Task DeleteAwbAsync(int id);

        Task<AwbItem> CreateItemAsync(AwbItem input);

        Task<AwbItem> UpdateItemAsync(AwbItem input);

        Task DeleteItemAsync(int id);

        Task<AwbAddress> CreateAddressAsync(AwbAddress input);

        Task<AwbAddress> UpdateAddressAsync(AwbAddress input);

        Task DeleteAddressAsync(int id);
    }
}