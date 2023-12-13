using Eaf.Application.Services.Dto;

namespace Eaf.Str.Airports.Dtos
{
    public class GetAirportInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; } = null;
    }
}