using System;
using System.Collections.Generic;

namespace AlbaranApi.Dto
{
    public class EntradaDto
    {
        public Guid EntradaId { get; set; }
        public DateTime CreationDate { get; set; }
        public string ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string Observation { get; set; }
        public List<EntradaProductoDto> EntradaProductosDto { get; set; }
    }
}