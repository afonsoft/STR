using Eaf.Application.Services.Dto;
using Eaf.AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Eaf.Str.Airports.Dtos
{
    [AutoMap(typeof(Airport))]
    public class AirportDto : EntityDto<int?>
    {
        [Required]
        [StringLength(Airport.MaxCodModelLength)]
        public string IATACode { get; set; }

        [Required]
        [StringLength(Airport.MaxModelLength)]
        public string NamePT { get; set; }

        [StringLength(Airport.MaxModelLength)]
        public string NameES { get; set; }

        [StringLength(Airport.MaxModelLength)]
        public string NameEN { get; set; }

        [Required]
        [StringLength(Airport.MaxShortModelLength)]
        public string ShortNamePT { get; set; }

        [StringLength(Airport.MaxShortModelLength)]
        public string ShortNameES { get; set; }

        [StringLength(Airport.MaxShortModelLength)]
        public string ShortNameEN { get; set; }

        [Required]
        [StringLength(Airport.MaxCodModelLength)]
        public string ICAOCode { get; set; }

        [Required]
        [StringLength(Airport.MaxCodModelLength)]
        public string CountryCode { get; set; }

        [StringLength(Airport.MaxCodModelLength)]
        public string TimeZone { get; set; }

        [StringLength(Airport.MaxCodModelLength)]
        public string Latitude { get; set; }

        [StringLength(Airport.MaxCodModelLength)]
        public string Longitude { get; set; }
    }
}