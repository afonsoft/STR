using Eaf.Domain.Repositories;
using Eaf.UI;
using System.Linq;
using System.Threading.Tasks;

namespace Eaf.Str.Airports
{
    public class AirportManager : StrDomainServiceBase, IAirportManager
    {
        private readonly IRepository<Airport> _repository;

        public AirportManager(IRepository<Airport> repository)
        {
            _repository = repository;
        }

        public IQueryable<Airport> Airports => _repository.GetAll();

        public async Task<Airport> CreateAsync(Airport input)
        {
            if (_repository.GetAll().Any(e => e.IATACode.ToLower() == input.IATACode.ToUpper()))
                throw new UserFriendlyException(L("Validate"));

            return await _repository.InsertAsync(input);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<Airport> GetByIATAAsync(string Iata)
        {
            return await _repository.FirstOrDefaultAsync(x => x.IATACode == Iata);
        }

        public async Task<Airport> GetByIdAsync(int id)
        {
            return await _repository.FirstOrDefaultAsync(id);
        }

        public async Task<Airport> UpdateAsync(Airport input)
        {
            return await _repository.UpdateAsync(input);
        }
    }
}