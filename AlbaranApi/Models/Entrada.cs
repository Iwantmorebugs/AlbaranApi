using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbaranApi.Models
{
    public class Entrada
    {
        internal Entrada(
            string entradaId,
            DateTime creationDate,
            string providerId,
            Guid productIdentity,
            decimal amountToBeAdded,
            string qrCodeData, byte[] qrCodeImage)
        {
            EntradaId = entradaId;
            CreationDate = creationDate;
            ProviderId = providerId;
            ProductIdentity = productIdentity;
            ProductAmount = amountToBeAdded;
            QrCodeData = qrCodeData;
            QrCodeImage = qrCodeImage;
        }

        internal Entrada()
        {
            /* Required by EF */
        }

        public string QrCodeData { get; set; }
        public string EntradaId { get; set; }
        public DateTime CreationDate { get; set; }
        public string ProviderId { get; set; }
        public Guid ProductIdentity { get; set; }
        public byte[] QrCodeImage { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal ProductAmount { get; set; }
    }
}