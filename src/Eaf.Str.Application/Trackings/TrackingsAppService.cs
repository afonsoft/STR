using Eaf.Application.Services.Dto;
using Eaf.Authorization;
using Eaf.Domain.Repositories;
using Eaf.Str.Authorization;
using Eaf.Str.AWBs;
using Eaf.Str.Trackings.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Eaf.Str.Trackings
{
    [EafAllowAnonymous]
    public class TrackingsAppService : StrAppServiceBase, ITrackingsAppService
    {
        private readonly IRepository<Tracking> _repository;

        public TrackingsAppService(IRepository<Tracking> repository)
        {
            _repository = repository;
        }

        [EafAuthorize(StrPermissions.Pages_Tracking_Create)]
        public async Task<int> Create(CreateOrEditTrackingDto input)
        {
            var tracking = ObjectMapper.Map<Tracking>(input);

            return await _repository.InsertAndGetIdAsync(tracking);
        }

        [EafAuthorize(StrPermissions.Pages_Tracking_Delete)]
        public Task Delete(int id)
        {
            return _repository.DeleteAsync(id);
        }

        [EafAuthorize(StrPermissions.Pages_Tracking)]
        public async Task<PagedResultDto<TrackingDto>> GetAll(GetTrackingInput input)
        {
            var query = _repository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => e.TrackingNumber.Contains(input.Filter));

            var total = await query.CountAsync();
            var items = await query.OrderBy(input.Sorting ?? "id asc").PageBy(input).ToListAsync();

            return new PagedResultDto<TrackingDto>(total, ObjectMapper.Map<List<TrackingDto>>(items));
        }

        [EafAuthorize(StrPermissions.Pages_Tracking_Edit)]
        public async Task<TrackingDto> GetForEdit(int id)
        {
            var tracking = await _repository.GetAll().FirstAsync(x => x.Id == id);
            return ObjectMapper.Map<TrackingDto>(tracking);
        }

        [EafAllowAnonymous]
        public async Task<IList<TrackingDto>> GetTracking(string trackingNumber)
        {
            var itens = await _repository.GetAll()
                .Where(x => x.TrackingNumber == trackingNumber)
                .OrderBy(x => x.Date)
                .ToListAsync();
            return ObjectMapper.Map<IList<TrackingDto>>(itens);
        }

        [EafAuthorize(StrPermissions.Pages_Tracking_Edit)]
        public async Task Update(CreateOrEditTrackingDto input)
        {
            var tracking = await _repository.GetAll().FirstAsync(x => x.Id == input.Id.Value);

            tracking.Description = input.Description;
            tracking.DescriptionType = input.DescriptionType;
            tracking.TrackingNumber = input.TrackingNumber;

            await _repository.UpdateAsync(tracking);
        }
    }
}