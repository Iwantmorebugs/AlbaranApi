using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlbaranApi.Models
{
    public class Entrada
    {
        internal Entrada(
                string entradaId,
                DateTime creationDate,
                string providerId,
                string albaranQrCodeData,
                string observation,
                List<EntradaProducto> entradaProductos)
            //  byte[] qrCodeImage)
        {
            EntradaId = entradaId;
            CreationDate = creationDate;
            ProviderId = providerId;
            AlbaranQrCodeData = albaranQrCodeData;
            Observation = observation;
            EntradaProductos = entradaProductos;
            //  QrCodeImage = qrCodeImage;
        }
        internal Entrada()
        {
           
            /* Required by EF */
        }
        public string AlbaranQrCodeData { get; set; }
        [Key]
        public string EntradaId { get; set; }
        public DateTime CreationDate { get; set; }
        public string ProviderId { get; set; }
        public string Observation { get; set; }
        public List<EntradaProducto> EntradaProductos { get; set; }

        // public byte[] QrCodeImage { get; set; }
    }
}