using Eaf.Application.Services;
using Eaf.Application.Services.Dto;
using Eaf.Str.Trackings.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eaf.Str.Trackings
{
    public interface ITrackingsAppService : IApplicationService
    {
        Task<PagedResultDto<TrackingDto>> GetAll(GetTrackingInput input);

        Task<IList<TrackingDto>> GetTracking(string trackingNumber);

        Task<TrackingDto> GetForEdit(EntityDto input);

        Task Create(CreateOrEditTrackingDto input);

        Task Update(CreateOrEditTrackingDto input);

        Task Delete(EntityDto input);
    }
}