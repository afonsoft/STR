using BarcodeStandard;
using Eaf.Application.Services.Dto;
using Eaf.Authorization;
using Eaf.Middleware.Dto;
using Eaf.Str.Authorization;
using Eaf.Str.Awbs.Dtos;
using Eaf.Str.Awbs.Exporting;
using Eaf.Str.AWBs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Eaf.Str.Awbs
{
    [EafAllowAnonymous]
    public class AwbAppService : StrAppServiceBase, IAwbAppService
    {
        private readonly IAwbManager _awbManager;
        private readonly IAwbExcelExporter _awbExcelExporter;

        public AwbAppService(IAwbManager awbManager, IAwbExcelExporter awbExcelExporter)
        {
            _awbManager = awbManager;
            _awbExcelExporter = awbExcelExporter;
        }

        [EafAuthorize(StrPermissions.Pages_Awb)]
        public async Task<AwbDto> CreateOrUpdate(CreateOrEditAwbDto input)
        {
            if (input.Id.HasValue)
                return await Update(input);
            else
                return await Create(input);
        }

        [EafAuthorize(StrPermissions.Pages_Awb_Create)]
        private async Task<AwbDto> Create(CreateOrEditAwbDto input)
        {
            var awb = ObjectMapper.Map<Awb>(input);

            if (awb.Code.IsNullOrEmpty())
                awb.Code = Guid.NewGuid().ToString("D");

            if (EafSession.TenantId != null && awb.TenantId <= 0)
                awb.TenantId = EafSession.TenantId.Value;

            return ObjectMapper.Map<AwbDto>(await _awbManager.CreateAwbAsync(awb));
        }

        [EafAuthorize(StrPermissions.Pages_Awb_Edit)]
        private async Task<AwbDto> Update(CreateOrEditAwbDto input)
        {
            var awb = await _awbManager.Awbs.FirstOrDefaultAsync(x => x.Id == input.Id);
            ObjectMapper.Map(input, awb);
            return ObjectMapper.Map<AwbDto>(await _awbManager.UpdateAwbAsync(awb));
        }

        [EafAuthorize(StrPermissions.Pages_Awb_Delete)]
        public async Task Delete(int id)
        {
            await _awbManager.DeleteAwbAsync(id);
        }

        [EafAllowAnonymous]
        public async Task<AwbDto> GetByTrackingNumber(string trackingNumber)
        {
            if (trackingNumber.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(trackingNumber), "trackingNumber is null or empty");

            if (trackingNumber.Length <= 5 || trackingNumber.Length >= 50)
                throw new ArgumentOutOfRangeException(nameof(trackingNumber), "trackingNumber has too few or too many characters.");

            var awb = await _awbManager.Awbs.FirstOrDefaultAsync(x => x.TrackingNumber.ToLower() == trackingNumber.ToLower());
            return ObjectMapper.Map<AwbDto>(awb);
        }

        [EafAllowAnonymous]
        public async Task<AwbDto> GetByCode(string code)
        {
            if (code.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(code), "code is null or empty");

            if (code.Length <= 1 || code.Length >= 50)
                throw new ArgumentOutOfRangeException(nameof(code), "code has too few or too many characters.");

            var awb = await _awbManager.Awbs.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower());
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

        public async Task<FileDto> getAwbToExcel(string filter)
        {
            var query = _awbManager.Awbs
              .WhereIf(!string.IsNullOrWhiteSpace(filter),
                  e => e.TrackingNumber.Contains(filter)
                    || e.Origin.Contains(filter)
                    || e.Destiny.Contains(filter));

            var items = await query.ToListAsync();
            return _awbExcelExporter.ExportToFile(ObjectMapper.Map<List<AwbDto>>(items));
        }

        [EafAllowAnonymous]
        public string GetBarCode([MaxLength(13)][MinLength(8)][NotNull] string barCode)
        {
            if (barCode.IsNullOrWhiteSpace() || barCode.IsNullOrEmpty())
                throw new EafException("barCode is null or empty");

            var code = new Barcode(barCode)
            {
                IncludeLabel = true,
                EncodedType = BarcodeStandard.Type.UpcA
            };

            return Convert.ToBase64String(code.EncodedImageBytes);
        }
    }
}