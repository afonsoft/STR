using Eaf.Application.Services.Dto;
using Eaf.Authorization;
using Eaf.Middleware.Dto;
using Eaf.Str.Airplanes.Dtos;
using Eaf.Str.Airplanes.Exporting;
using Eaf.Str.Airplanes.Jobs;
using Eaf.Str.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Eaf.Str.Airplanes
{
    [EafAuthorize(StrPermissions.Pages_Airplanes)]
    public class AirplanesAppService : StrAppServiceBase, IAirplanesAppService
    {
        private readonly IAirplaneJob _airplaneJob;
        private readonly IAirplaneManager _airplaneManager;
        private readonly IAirplanesExcelExporter _airplanesExcelExporter;

        public AirplanesAppService(
            IAirplaneJob airplaneJob,
            IAirplaneManager airplaneManager,
            IAirplanesExcelExporter airplanesExcelExporter
        )
        {
            LocalizationSourceName = StrConsts.LocalizationSourceName;

            _airplaneJob = airplaneJob;
            _airplaneManager = airplaneManager;
            _airplanesExcelExporter = airplanesExcelExporter;
        }

        public async Task<PagedResultDto<AirplaneDto>> GetAll(GetAirplanesInput input)
        {
            var query = _airplaneManager.Airplanes
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => e.Number.Contains(input.Filter) || e.Model.Contains(input.Filter));

            var total = await query.CountAsync();
            var items = await query.OrderBy(input.Sorting ?? "id asc").PageBy(input).ToListAsync();

            return new PagedResultDto<AirplaneDto>(total, ObjectMapper.Map<List<AirplaneDto>>(items));
        }

        [EafAuthorize(StrPermissions.Pages_Airplanes_Edit)]
        public async Task<CreateOrEditAirplaneDto> GetAirplaneForEdit(EntityDto input)
        {
            var airplane = await _airplaneManager.GetByIdAsync(input.Id);
            return ObjectMapper.Map<CreateOrEditAirplaneDto>(airplane);
        }

        public async Task CreateOrEdit(CreateOrEditAirplaneDto input)
        {
            if (input.Id.HasValue)
                await Update(input);
            else
                await Create(input);
        }

        [EafAuthorize(StrPermissions.Pages_Airplanes_Create)]
        private async Task Create(CreateOrEditAirplaneDto input)
        {
            var airplane = ObjectMapper.Map<Airplane>(input);

            if (EafSession.TenantId != null)
                airplane.TenantId = EafSession.TenantId;

            await _airplaneManager.CreateAsync(airplane);
        }

        [EafAuthorize(StrPermissions.Pages_Airplanes_Edit)]
        private async Task Update(CreateOrEditAirplaneDto input)
        {
            var airplane = await _airplaneManager.GetByIdAsync(input.Id.Value);
            ObjectMapper.Map(input, airplane);
            await _airplaneManager.UpdateAsync(airplane);
        }

        [EafAuthorize(StrPermissions.Pages_Airplanes_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _airplaneManager.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetAirplanesToExcel()
        {
            var items = await _airplaneManager.Airplanes.ToListAsync();
            return _airplanesExcelExporter.ExportToFile(ObjectMapper.Map<List<AirplaneDto>>(items));
        }

        public Task StartJob()
        {
            return _airplaneJob.StartProcess();
        }
    }
}