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

        public EntradaProducto(string trace, List<Container> container, decimal productAmount, Guid productIdentity,
            string productName, string productQrData, decimal productPrice = 0)
        {
            Trace = trace;
            Container = container;
            ProductAmount = productAmount;
            ProductPrice = productPrice;
            ProductIdentity = productIdentity;
            ProductName = productName;
            ProductQrData = productQrData;
        }

        public string Trace { get; set; }
        public  List<Container> Container{ get; set; }

        [Column(TypeName = "decimal(18,4)")] 
        public decimal ProductAmount { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal ProductPrice { get; set; }
        [Key]
        public Guid ProductIdentity { get; set; }
        public string ProductName { get; set; }
        public string ProductQrData { get; set; }

    }
}
