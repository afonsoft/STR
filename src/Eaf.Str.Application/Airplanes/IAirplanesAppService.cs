using System.Threading.Tasks;
using Eaf.Application.Services;
using Eaf.Application.Services.Dto;
using Eaf.Middleware.Dto;
using Eaf.Str.Airplanes.Dtos;

namespace Eaf.Str.Airplanes
{
    public interface IAirplanesAppService : IApplicationService 
    {
        Task<PagedResultDto<AirplaneDto>> GetAll(GetAirplanesInput input);

        Task<CreateOrEditAirplaneDto> GetAirplaneForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAirplaneDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetAirplanesToExcel();

        Task StartJob();
    }
}