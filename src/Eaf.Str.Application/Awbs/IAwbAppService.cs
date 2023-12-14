using Eaf.Application.Services;
using Eaf.Application.Services.Dto;
using Eaf.Str.Awbs.Dtos;
using System.Threading.Tasks;

namespace Eaf.Str.Awbs
{
    public interface IAwbAppService : IApplicationService
    {
        Task<PagedResultDto<AwbDto>> GetAll(GetAwbInput input);

        Task<AwbDto> Get(string trackingNumber);

        Task<CreateOrEditAwbDto> GetForEdit(int id);

        Task<string> CreateOrUpdate(CreateOrEditAwbDto input);

        Task Delete(int id);
    }
}