using Eaf.Auditing;
using Eaf.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eaf.Str.Airports
{
    [Table("Airports")]
    [Audited]
    public class Airport : FullAuditedEntity
    {
        public const int MaxShortModelLength = 256;
        public const int MaxModelLength = 512;
        public const int MaxCodModelLength = 20;

        [Required]
        [StringLength(MaxCodModelLength)]
        public string IATACode { get; set; }

        [Required]
        [StringLength(MaxModelLength)]
        public string NamePT { get; set; }

        [StringLength(MaxModelLength)]
        public string NameES { get; set; }

        [StringLength(MaxModelLength)]
        public string NameEN { get; set; }

        [Required]
        [StringLength(MaxShortModelLength)]
        public string ShortNamePT { get; set; }

        [StringLength(MaxShortModelLength)]
        public string ShortNameES { get; set; }

        [StringLength(MaxShortModelLength)]
        public string ShortNameEN { get; set; }

        [Required]
        [StringLength(MaxCodModelLength)]
        public string ICAOCode { get; set; }

        [Required]
        [StringLength(MaxCodModelLength)]
        public string CountryCode { get; set; }

        [StringLength(MaxCodModelLength)]
        public string TimeZone { get; set; }

        [StringLength(MaxCodModelLength)]
        public string Latitude { get; set; }

        [StringLength(MaxCodModelLength)]
        public string Longitude { get; set; }
    }
}