using System;
using System.ComponentModel.DataAnnotations.Schema;
using QRCoder;

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
            string qrCodeData)
        {
           
            EntradaId = entradaId;
            CreationDate = creationDate;
            ProviderId = providerId;
            ProductIdentity = productIdentity;
            ProductAmount = amountToBeAdded;
            QrCodeData = qrCodeData;
        }

        public string QrCodeData { get; set; }
        public string EntradaId { get; set; }
        public DateTime CreationDate { get; set; }
        public string ProviderId { get; set; }
        public Guid ProductIdentity { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal ProductAmount { get; set; }

        internal Entrada()
        {
            /* Required by EF */
        }
    }
}
