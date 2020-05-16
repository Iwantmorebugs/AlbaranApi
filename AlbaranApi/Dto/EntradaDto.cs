using AlbaranApi.Models;
using System;
using System.Collections.Generic;

namespace AlbaranApi.Dto
{
    public class EntradaDto
    {
        public string EntradaId { get; set; }
        public DateTime CreationDate { get; set; }
        public string ProviderId { get; set; }
        public string Observation { get; set; }
        public List<EntradaProductoDto> EntradaProductosDto { get; set; }

    }
}