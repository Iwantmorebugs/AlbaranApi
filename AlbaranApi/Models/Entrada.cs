using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlbaranApi.Models
{
    public class Entrada
    {
        internal Entrada(
                Guid id,
                DateTime creationDate,
                string providerId,
                string albaranQrCodeData,
                string observation,
                List<EntradaProducto> entradaProductos, string providerName)
            //  byte[] qrCodeImage)
        {
            Id = id;
            CreationDate = creationDate;
            ProviderId = providerId;
            AlbaranQrCodeData = albaranQrCodeData;
            Observation = observation;
            EntradaProductos = entradaProductos;
            ProviderName = providerName;
            //  QrCodeImage = qrCodeImage;
        }
        internal Entrada()
        {
            /* Required by EF */
        }

        public string AlbaranQrCodeData { get; set; }

        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }
        public string ProviderId { get; set; }
        public string Observation { get; set; }
        public List<EntradaProducto> EntradaProductos { get; set; }
        public string ProviderName { get; set; }

        // public byte[] QrCodeImage { get; set; }
    }
}