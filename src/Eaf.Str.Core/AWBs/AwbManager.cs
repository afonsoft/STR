using Eaf.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Eaf.Str.AWBs
{
    public class AwbManager : StrDomainServiceBase, IAwbManager
    {
        private readonly IRepository<AwbAddress> _awbAddressRepository;
        private readonly IRepository<AwbItem> _awbItemRepository;
        private readonly IRepository<Awb> _awbRepository;

        public AwbManager(IRepository<AwbAddress> awbAddressRepository,
            IRepository<AwbItem> awbItemRepository,
            IRepository<Awb> awbRepository)
        {
            _awbAddressRepository = awbAddressRepository;
            _awbItemRepository = awbItemRepository;
            _awbRepository = awbRepository;
        }

        public IQueryable<AwbAddress> AwbAddress => _awbAddressRepository.GetAll();

        public IQueryable<AwbItem> AwbItems => _awbItemRepository.GetAll();

        public IQueryable<Awb> Awbs => _awbRepository.GetAll()
                                    .Include(x => x.Sender)
                                    .Include(x => x.Recipient)
                                    .Include(x => x.Itens);

        public Task<AwbAddress> CreateAddressAsync(AwbAddress input)
        {
            return _awbAddressRepository.InsertOrUpdateAsync(input);
        }

        public Task<Awb> CreateAwbAsync(Awb input)
        {
            return _awbRepository.InsertOrUpdateAsync(input);
        }

        public Task<AwbItem> CreateItemAsync(AwbItem input)
        {
            return _awbItemRepository.InsertOrUpdateAsync(input);
        }

        public Task DeleteAddressAsync(int id)
        {
            return _awbAddressRepository.DeleteAsync(id);
        }

        public Task DeleteAwbAsync(int id)
        {
            return _awbRepository.DeleteAsync(id);
        }

        public Task DeleteItemAsync(int id)
        {
            return _awbItemRepository.DeleteAsync(id);
        }

        public Task<AwbAddress> UpdateAddressAsync(AwbAddress input)
        {
            return _awbAddressRepository.UpdateAsync(input);
        }

        public Task<Awb> UpdateAwbAsync(Awb input)
        {
            return _awbRepository.UpdateAsync(input);
        }

        public Task<AwbItem> UpdateItemAsync(AwbItem input)
        {
            return _awbItemRepository.UpdateAsync(input);
        }
    }
}