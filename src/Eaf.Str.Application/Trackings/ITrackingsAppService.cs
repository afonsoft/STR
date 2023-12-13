using Eaf.Application.Services;
using Eaf.Application.Services.Dto;
using Eaf.Str.Trackings.Dtos;
using System.Threading.Tasks;

namespace Eaf.Str.Trackings
{
    public interface ITrackingsAppService : IApplicationService
    {
        Task<PagedResultDto<TrackingDto>> GetAll(GetTrackingInput input);

        Task<TrackingDto> GetAirplaneForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTrackingDto input);

        Task Delete(EntityDto input);
    }
}