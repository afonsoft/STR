using Eaf.Application.Services.Dto;
using Eaf.Authorization;
using Eaf.Str.Airports.Dtos;
using Eaf.Str.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eaf.Str.Airports
{
    [EafAuthorize(StrPermissions.Pages_Airports)]
    public class AirportsAppService : StrAppServiceBase, IAirportsAppService
    {
        private readonly IAirportManager _airportManager;

        public AirportsAppService(IAirportManager airportManager)
        {
            LocalizationSourceName = StrConsts.LocalizationSourceName;
            _airportManager = airportManager;
        }

        public async Task CreateOrEdit(AirportDto input)
        {
            if (input.Id.HasValue)
                await Update(input);
            else
                await Create(input);
        }

        [EafAuthorize(StrPermissions.Pages_Airports_Create)]
        private async Task Create(AirportDto input)
        {
            var airport = ObjectMapper.Map<Airport>(input);

            await _airportManager.CreateAsync(airport);
        }

        [EafAuthorize(StrPermissions.Pages_Airports_Edit)]
        private async Task Update(AirportDto input)
        {
            var airport = await _airportManager.GetByIdAsync(input.Id.Value);
            ObjectMapper.Map(input, airport);
            await _airportManager.UpdateAsync(airport);
        }

        [EafAuthorize(StrPermissions.Pages_Airports_Delete)]
        public async Task Delete(EntityDto input)
        {
            var airport = ObjectMapper.Map<Airport>(input);
            await _airportManager.CreateAsync(airport);
        }

        public async Task<AirportDto> GetAirplaneForEdit(EntityDto input)
        {
            var airport = await _airportManager.GetByIdAsync(input.Id);
            return ObjectMapper.Map<AirportDto>(airport);
        }

        public async Task<PagedResultDto<AirportDto>> GetAll(GetAirportInput input)
        {
            var query = _airportManager.Airports
              .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                  e => e.IATACode.Contains(input.Filter) || e.NamePT.Contains(input.Filter)
                  || e.CountryCode.Contains(input.Filter));

            var total = await query.CountAsync();
            var items = await query.OrderBy(input.Sorting ?? "id asc").PageBy(input).ToListAsync();

            return new PagedResultDto<AirportDto>(total, ObjectMapper.Map<List<AirportDto>>(items));
        }

        public async Task<AirportDto> GetByIATA(string iata)
        {
            var airport = await _airportManager.GetByIATAAsync(iata);
            return ObjectMapper.Map<AirportDto>(airport);
        }
    }
}