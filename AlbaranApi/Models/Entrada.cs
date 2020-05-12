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
                string qrCodeData, decimal productPrice, string productName, string brand, string picture,
                string category)
            //  byte[] qrCodeImage)
        {
            EntradaId = entradaId;
            CreationDate = creationDate;
            ProviderId = providerId;
            ProductIdentity = productIdentity;
            ProductAmount = amountToBeAdded;
            QrCodeData = qrCodeData;
            ProductPrice = productPrice;
            ProductName = productName;
            Brand = brand;
            Picture = picture;
            Category = category;
            //  QrCodeImage = qrCodeImage;
        }

        internal Entrada()
        {
            /* Required by EF */
        }

        public string QrCodeData { get; set; }
        public string EntradaId { get; set; }
        public DateTime CreationDate { get; set; }
        public string ProviderId { get; set; }
        public string ProductName { get; set; }
        public Guid ProductIdentity { get; set; }

        // public byte[] QrCodeImage { get; set; }

        [Column(TypeName = "decimal(18,4)")] public decimal ProductAmount { get; set; }

        [Column(TypeName = "decimal(18,4)")] public decimal ProductPrice { get; set; }

        public string Brand { get; set; }
        public string Picture { get; set; }
        public string Category { get; set; }
    }
}