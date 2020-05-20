using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbaranApi.Models
{
    public class EntradaProducto
    {
        public EntradaProducto()
        {
        }

        public EntradaProducto(string trace, decimal productAmount, Guid productIdentity,
            string productName, string productQrData, decimal productPrice = 0)
        {
            Trace = trace;
            ProductAmount = productAmount;
            ProductPrice = productPrice;
            ProductIdentity = productIdentity;
            ProductName = productName;
            ProductQrData = productQrData;
        }

        public string Trace { get; set; }
        
       public decimal ProductAmount { get; set; }

        public decimal ProductPrice { get; set; }

        public Guid ProductIdentity { get; set; }

        public string ProductName { get; set; }
        public string ProductQrData { get; set; }
    }
}