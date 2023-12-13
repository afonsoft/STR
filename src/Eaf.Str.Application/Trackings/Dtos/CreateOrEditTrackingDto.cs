using Eaf.Application.Services.Dto;
using Eaf.Str.Airplanes;
using System.ComponentModel.DataAnnotations;

namespace Eaf.Str.Trackings.Dtos
{
    public class CreateOrEditTrackingDto : EntityDto<int?>
    {
        [Required]
        public string Number { get; set; }
    }
}