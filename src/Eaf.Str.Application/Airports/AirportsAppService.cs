using Eaf.Application.Services.Dto;
using Eaf.Authorization;
using Eaf.Str.Airports.Dtos;
using Eaf.Str.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eaf.Str.Airports
{
    [EafAllowAnonymous]
    public class AirportsAppService : StrAppServiceBase, IAirportsAppService
    {
        private readonly IAirportManager _airportManager;

        public AirportsAppService(IAirportManager airportManager)
        {
            LocalizationSourceName = StrConsts.LocalizationSourceName;
            _airportManager = airportManager;
        }

        [EafAuthorize(StrPermissions.Pages_Airports)]
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

        [EafAuthorize(StrPermissions.Pages_Airports)]
        public async Task<AirportDto> GetAirportForEdit(EntityDto input)
        {
            var airport = await _airportManager.GetByIdAsync(input.Id);
            return ObjectMapper.Map<AirportDto>(airport);
        }

        [EafAuthorize(StrPermissions.Pages_Airports)]
        public async Task<PagedResultDto<AirportDto>> GetAll(GetAirportInput input)
        {
            var query = _airportManager.Airports
              .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                  e => e.IATACode.Contains(input.Filter) || e.NamePT.Contains(input.Filter)
                  || e.CountryCode.Contains(input.Filter));

            var total = await query.CountAsync();
            var items = await query.OrderBy(input.Sorting ?? "NamePT asc").PageBy(input).ToListAsync();

            return new PagedResultDto<AirportDto>(total, ObjectMapper.Map<List<AirportDto>>(items));
        }

        [EafAllowAnonymous]
        public async Task<AirportDto> GetByIATA(string iata)
        {
            var airport = await _airportManager.GetByIATAAsync(iata);
            return ObjectMapper.Map<AirportDto>(airport);
        }

        [EafAllowAnonymous]
        public async Task<List<AirportDto>> GetByNameOrIata(string nameOrIata)
        {
            if (nameOrIata.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(nameOrIata), "nameOrIata is null or empty");

            if (nameOrIata.Length <= 1 || nameOrIata.Length >= 50)
                throw new ArgumentOutOfRangeException(nameof(nameOrIata), "nameOrIata has too few or too many characters.");

            var query = await _airportManager.Airports
              .Where(e => e.IATACode.Contains(nameOrIata) || e.NamePT.Contains(nameOrIata)
                  || e.NameES.Contains(nameOrIata) || e.NameEN.Contains(nameOrIata)
                  || e.CountryCode.Contains(nameOrIata))
              .ToListAsync();

            return ObjectMapper.Map<List<AirportDto>>(query);
        }
    }
}