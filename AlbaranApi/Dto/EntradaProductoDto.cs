using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AlbaranApi.Models;

namespace AlbaranApi.Dto
{
    public class EntradaProductoDto
    {
        [Column(TypeName = "decimal(18,4)")] public decimal ProductAmount { get; set; }

        [Column(TypeName = "decimal(18,4)")] public decimal ProductPrice { get; set; }

        public Guid ProductIdentity { get; set; }
        public string ProductName { get; set; }
    }
}