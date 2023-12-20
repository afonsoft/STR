using Eaf.Auditing;
using Eaf.Domain.Entities;
using Eaf.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eaf.Str.AWBs
{
    [Index(nameof(ZipCode), IsUnique = false)]
    [Table("AwbAddress")]
    [Audited]
    public class AwbAddress : FullAuditedEntity, IMayHaveTenant
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

    [Index(nameof(Invoice), IsUnique = false)]
    [Table("AwbItens")]
    [Audited]
    public class AwbItem : FullAuditedEntity, IMayHaveTenant
    {
        public int? AwbId { get; set; }

        [ForeignKey(nameof(AwbId))]
        public Awb Awb { get; set; }

        /// <summary>
        /// Peso
        /// </summary>
        [StringLength(50)]
        public string Weight { get; set; }

        /// <summary>
        /// Quantidades
        /// </summary>
        [Range(0, 9999)]
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

    [Index(nameof(TrackingNumber), IsUnique = true)]
    [Table("Awb")]
    [Audited]
    public class Awb : FullAuditedEntity, IMayHaveTenant
    {
        /// <summary>
        ///  Numero do Rastreio
        /// </summary>
        [Required]
        [StringLength(50)]
        public string TrackingNumber { get; set; }

        /// <summary>
        ///  Codigo do AWB Interno
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        public int RecipientId { get; set; }

        /// <summary>
        /// Destinatário
        /// </summary>
        [Required]
        [ForeignKey(nameof(RecipientId))]
        public AwbAddress Recipient { get; set; }

        public int SenderId { get; set; }

        /// <summary>
        /// Remetente
        /// </summary>
        [Required]
        [ForeignKey(nameof(SenderId))]
        public AwbAddress Sender { get; set; }

        /// <summary>
        /// Origem (CGH)
        /// </summary>
        [StringLength(10)]
        public string Origin { get; set; }

        /// <summary>
        ///  Destino (RIO)
        /// </summary>
        [StringLength(10)]
        public string Destiny { get; set; }

        /// <summary>
        /// Pessoa que recebeu
        /// </summary>
        [StringLength(512)]
        public string ReceivedName { get; set; }

        /// <summary>
        /// Data Recebido
        /// </summary>
        public DateTimeOffset? ReceivedDate { get; set; }

        /// <summary>
        /// Documento do recebido
        /// </summary>
        [StringLength(128)]
        public string ReceivedDocument { get; set; }

        public virtual ICollection<AwbItem> Itens { get; set; }
        public int? TenantId { get; set; }
    }

    [Index(nameof(TrackingNumber), nameof(Date), IsUnique = false, AllDescending = true)]
    [Table("Tracking")]
    [Audited]
    public class Tracking : FullAuditedEntity
    {
        /// <summary>
        ///  Numero do Rastreio
        /// </summary>
        [Required]
        [StringLength(50)]
        public string TrackingNumber { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(128)]
        public string DescriptionType { get; set; }

        public DateTimeOffset Date { get; set; }
    }
}