using Eaf.Domain.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Eaf.Str.Airports
{
    public interface IAirportManager : IDomainService
    {
        IQueryable<Airport> Airports { get; }

        Task<Airport> CreateAsync(Airport input);

        Task<Airport> UpdateAsync(Airport input);

        Task DeleteAsync(int id);

        Task<Airport> GetByIdAsync(int id);

        Task<Airport> GetByIATAAsync(string Iata);

        Task<string> StartProcess();
    }
}