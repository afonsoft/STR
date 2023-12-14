using Eaf.Application.Services.Dto;
using Eaf.AutoMapper;
using Eaf.Str.AWBs;
using System;
using System.ComponentModel.DataAnnotations;

namespace Eaf.Str.Awbs.Dtos
{
    [AutoMap(typeof(AwbItem))]
    public class AwbItemDto : FullAuditedEntityDto
    {
        public int? AwbId { get; set; }

        /// <summary>
        /// Peso
        /// </summary>
        [StringLength(50)]
        public string Weight { get; set; }

        /// <summary>
        /// Quantidades
        /// </summary>
        [Range(1, 9999)]
        public int Quantities { get; set; }

        /// <summary>
        /// Nota Fiscal
        /// </summary>
        [StringLength(50)]
        public string Invoice { get; set; }

        /// <summary>
        ///  Tipo Embalagem
        /// </summary>
        [StringLength(128)]
        public string PackagingType { get; set; }

        /// <summary>
        /// Material
        /// </summary>
        [StringLength(128)]
        public string MaterialType { get; set; }

        public int? TenantId { get; set; }
    }
}