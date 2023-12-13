using Eaf.Application.Services;
using Eaf.Application.Services.Dto;
using Eaf.Str.Airports.Dtos;
using System.Threading.Tasks;

namespace Eaf.Str.Airports
{
    public interface IAirportsAppService : IApplicationService
    {
        Task<PagedResultDto<AirportDto>> GetAll(GetAirportInput input);

        Task<AirportDto> GetByIATA(string iata);

        Task<AirportDto> GetAirplaneForEdit(EntityDto input);

        Task CreateOrEdit(AirportDto input);

        Task Delete(EntityDto input);
    }
}