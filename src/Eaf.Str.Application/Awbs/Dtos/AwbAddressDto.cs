using Eaf.Application.Services.Dto;
using Eaf.AutoMapper;
using Eaf.Str.AWBs;
using System.ComponentModel.DataAnnotations;

namespace Eaf.Str.Awbs.Dtos
{
    [AutoMap(typeof(AwbAddress))]
    public class AwbAddressDto : FullAuditedEntityDto
    {
        /// <summary>
        /// Cep
        /// </summary>
        [Required]
        [StringLength(10)]
        public string ZipCode { get; set; }

        /// <summary>
        /// logradouro
        /// </summary>
        [Required]
        [StringLength(512)]
        public string Street { get; set; }

        /// <summary>
        /// bairro
        /// </summary>
        [Required]
        [StringLength(256)]
        public string Neighborhood { get; set; }

        /// <summary>
        /// localidade
        /// </summary>
        [Required]
        [StringLength(256)]
        public string City { get; set; }

        /// <summary>
        /// uf
        /// </summary>
        [Required]
        [StringLength(4)]
        public string State { get; set; }

        /// <summary>
        /// complemento
        /// </summary>
        [StringLength(256)]
        public string Complement { get; set; }

        /// <summary>
        /// observação
        /// </summary>
        [StringLength(512)]
        public string Observation { get; set; }

        /// <summary>
        /// Nome da Pessoa
        /// </summary>
        [StringLength(512)]
        public string PersonName { get; set; }

        public int? TenantId { get; set; }
    }
}