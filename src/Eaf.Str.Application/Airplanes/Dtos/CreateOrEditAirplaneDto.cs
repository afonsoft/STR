using Eaf.Application.Services.Dto;
using Eaf.AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Eaf.Str.Airplanes.Dtos
{
    [AutoMap(typeof(Airplane))]
    public class CreateOrEditAirplaneDto : EntityDto<int?>
    {
        [Required]
        public string Number { get; set; }

        [Required]
        [StringLength(Airplane.MaxModelLength)]
        public string Model { get; set; }
    }
}