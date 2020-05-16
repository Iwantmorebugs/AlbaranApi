using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using AlbaranApi.Models;

namespace AlbaranApi.Dto
{
    public class EntradaProductoDto
    {
        public List<Container> Container { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal ProductAmount { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal ProductPrice { get; set; }
        public Guid ProductIdentity { get; set; }
        public string ProductName { get; set; }
    }
}
