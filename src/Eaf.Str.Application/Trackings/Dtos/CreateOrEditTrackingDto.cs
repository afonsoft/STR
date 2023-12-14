using Eaf.Application.Services.Dto;
using Eaf.AutoMapper;
using Eaf.Str.AWBs;
using System.ComponentModel.DataAnnotations;

namespace Eaf.Str.Trackings.Dtos
{
    [AutoMap(typeof(Tracking))]
    public class CreateOrEditTrackingDto : EntityDto<int?>
    {
        [Required]
        [StringLength(50)]
        public string TrackingNumber { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(128)]
        public string DescriptionType { get; set; }
    }
}