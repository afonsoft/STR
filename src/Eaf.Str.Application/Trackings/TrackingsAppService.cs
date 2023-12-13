using Eaf.Application.Services.Dto;
using Eaf.Str.Trackings.Dtos;
using System;
using System.Threading.Tasks;

namespace Eaf.Str.Trackings
{
    public class TrackingsAppService : ITrackingsAppService
    {
        public Task CreateOrEdit(CreateOrEditTrackingDto input)
        {
            throw new NotImplementedException();
        }

        public Task Delete(EntityDto input)
        {
            throw new NotImplementedException();
        }

        public Task<TrackingDto> GetAirplaneForEdit(EntityDto input)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<TrackingDto>> GetAll(GetTrackingInput input)
        {
            throw new NotImplementedException();
        }
    }
}