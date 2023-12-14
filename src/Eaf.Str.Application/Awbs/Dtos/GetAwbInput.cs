using Eaf.Application.Services.Dto;

namespace Eaf.Str.Awbs.Dtos
{
    public class GetAwbInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; } = null;
    }
}