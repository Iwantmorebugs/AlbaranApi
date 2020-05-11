using System;
using QRCoder;

namespace AlbaranApi.Models
{
    public class Entrada
    {
        internal Entrada(QRCode qrCode, string entradaId, DateTime creationDate, string providerId, Guid productIdentity, decimal amountToBeAdded)
        {
            QrCode = qrCode;
            EntradaId = entradaId;
            CreationDate = creationDate;
            ProviderId = providerId;
            ProductIdentity = productIdentity;
            ProductAmount = amountToBeAdded;
        }

        public QRCode QrCode { get; set; }
        public string EntradaId { get; set; }
        public DateTime CreationDate { get; set; }
        public string ProviderId { get; set; }
        public Guid ProductIdentity { get; set; }
        public decimal ProductAmount { get; set; }

        internal Entrada()
        {
            /* Required by EF */
        }
    }
}
