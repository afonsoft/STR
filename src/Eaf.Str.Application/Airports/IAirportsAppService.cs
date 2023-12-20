using Eaf.Application.Services;
using Eaf.Application.Services.Dto;
using Eaf.Str.Airports.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eaf.Str.Airports
{
    public interface IAirportsAppService : IApplicationService
    {
        Task<PagedResultDto<AirportDto>> GetAll(GetAirportInput input);

        Task<AirportDto> GetByIATA(string iata);

        Task<AirportDto> GetAirportForEdit(EntityDto input);

        Task CreateOrEdit(AirportDto input);

        Task Delete(EntityDto input);

        Task<List<AirportDto>> GetByNameOrIata(string nameOrIata);
    }
}