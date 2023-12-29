using Eaf.Application.Services.Dto;
using Eaf.AutoMapper;
using Eaf.Str.AWBs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Eaf.Str.Awbs.Dtos
{
    [AutoMap(typeof(Awb))]
    public class CreateOrEditAwbDto : EntityDto<int?>
    {
        public string BarCode { get; set; }

        /// <summary>
        ///  Numero do Rastreio
        /// </summary>
        [StringLength(50)]
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Destinatário
        /// </summary>
        [Required]
        public AwbAddressDto Recipient { get; set; }

        /// <summary>
        /// Remetente
        /// </summary>
        [Required]
        public AwbAddressDto Sender { get; set; }

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

        public virtual IList<AwbItemDto> Itens { get; set; }

        /// <summary>
        /// Last modification date of this entity.
        /// </summary>
        public DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// Creation date of this entity.
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}