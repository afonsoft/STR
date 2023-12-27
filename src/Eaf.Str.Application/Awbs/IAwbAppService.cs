using Eaf.Application.Services;
using Eaf.Application.Services.Dto;
using Eaf.Str.Awbs.Dtos;
using System.Threading.Tasks;

namespace Eaf.Str.Awbs
{
    public interface IAwbAppService : IApplicationService
    {
        Task<PagedResultDto<AwbDto>> GetAll(GetAwbInput input);

        Task<AwbDto> GetByTrackingNumber(string trackingNumber);

        Task<AwbDto> GetByCode(string code);

        Task<CreateOrEditAwbDto> GetForEdit(int id);

        Task<AwbDto> CreateOrUpdate(CreateOrEditAwbDto input);

        Task Delete(int id);

        string GetBarCode(string barCode);
    }
}