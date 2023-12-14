using Eaf.Application.Services.Dto;
using Eaf.Authorization;
using Eaf.Str.Authorization;
using Eaf.Str.Trackings.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eaf.Str.Trackings
{
    [EafAllowAnonymous]
    public class TrackingsAppService : StrAppServiceBase, ITrackingsAppService
    {
        [EafAuthorize(StrPermissions.Pages_Tracking_Create)]
        public Task Create(CreateOrEditTrackingDto input)
        {
            throw new NotImplementedException();
        }

        [EafAuthorize(StrPermissions.Pages_Tracking_Delete)]
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        [EafAuthorize(StrPermissions.Pages_Tracking)]
        public Task<PagedResultDto<TrackingDto>> GetAll(GetTrackingInput input)
        {
            throw new NotImplementedException();
        }

        [EafAuthorize(StrPermissions.Pages_Tracking_Edit)]
        public Task<TrackingDto> GetForEdit(int id)
        {
            throw new NotImplementedException();
        }

        [EafAllowAnonymous]
        public Task<IList<TrackingDto>> GetTracking(string trackingNumber)
        {
            throw new NotImplementedException();
        }

        [EafAuthorize(StrPermissions.Pages_Tracking_Edit)]
        public Task Update(CreateOrEditTrackingDto input)
        {
            throw new NotImplementedException();
        }
    }
}