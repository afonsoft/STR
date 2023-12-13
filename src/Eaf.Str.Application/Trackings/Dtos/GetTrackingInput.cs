using Eaf.Application.Services.Dto;

namespace Eaf.Str.Trackings.Dtos
{
    public class GetTrackingInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; } = null;
    }
}