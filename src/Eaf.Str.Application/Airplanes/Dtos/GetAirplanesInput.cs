using Eaf.Application.Services.Dto;
using System;

namespace Eaf.Str.Airplanes.Dtos
{
    public class GetAirplanesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; } = null;
    }
}