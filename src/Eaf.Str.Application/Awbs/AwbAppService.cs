using Eaf.Application.Services.Dto;
using Eaf.Authorization;
using Eaf.Str.Authorization;
using Eaf.Str.Awbs.Dtos;
using Eaf.Str.AWBs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Eaf.Str.Awbs
{
    [EafAllowAnonymous]
    public class AwbAppService : StrAppServiceBase, IAwbAppService
    {
        private readonly IAwbManager _awbManager;

        public AwbAppService(IAwbManager awbManager)
        {
            _awbManager = awbManager;
        }

        public async Task<string> CreateOrUpdate(CreateOrEditAwbDto input)
        {
            if (input.Id.HasValue)
                return await Update(input);
            else
                return await Create(input);
        }

        [EafAuthorize(StrPermissions.Pages_Awb_Create)]
        private async Task<string> Create(CreateOrEditAwbDto input)
        {
            var awb = ObjectMapper.Map<Awb>(input);

            if (EafSession.TenantId != null)
                awb.TenantId = EafSession.TenantId;

            await _awbManager.CreateAwbAsync(awb);
            return awb.TrackingNumber;
        }

        [EafAuthorize(StrPermissions.Pages_Awb_Edit)]
        private async Task<string> Update(CreateOrEditAwbDto input)
        {
            var awb = await _awbManager.Awbs.FirstOrDefaultAsync(x => x.Id == input.Id);
            ObjectMapper.Map(input, awb);
            await _awbManager.UpdateAwbAsync(awb);
            return awb.TrackingNumber;
        }

        [EafAuthorize(StrPermissions.Pages_Awb_Delete)]
        public async Task Delete(int id)
        {
            await _awbManager.DeleteAwbAsync(id);
        }

        [EafAuthorize(StrPermissions.Pages_Awb)]
        public async Task<AwbDto> Get(string trackingNumber)
        {
            var awb = await _awbManager.Awbs.FirstOrDefaultAsync(x => x.TrackingNumber.ToLower() == trackingNumber.ToLower());
            return ObjectMapper.Map<AwbDto>(awb);
        }

        [EafAuthorize(StrPermissions.Pages_Awb)]
        public async Task<PagedResultDto<AwbDto>> GetAll(GetAwbInput input)
        {
            var query = _awbManager.Awbs
              .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                  e => e.TrackingNumber.Contains(input.Filter)
                    || e.Origin.Contains(input.Filter)
                    || e.Destiny.Contains(input.Filter));

            var total = await query.CountAsync();
            var items = await query.OrderBy(input.Sorting ?? "id asc").PageBy(input).ToListAsync();

            return new PagedResultDto<AwbDto>(total, ObjectMapper.Map<List<AwbDto>>(items));
        }

        [EafAuthorize(StrPermissions.Pages_Awb_Edit)]
        public async Task<CreateOrEditAwbDto> GetForEdit(int id)
        {
            var awb = await _awbManager.Awbs.FirstOrDefaultAsync(x => x.Id == id);
            return ObjectMapper.Map<CreateOrEditAwbDto>(awb);
        }
    }
}