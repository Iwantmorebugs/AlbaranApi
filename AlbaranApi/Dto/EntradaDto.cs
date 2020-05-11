using System;

namespace AlbaranApi.Dto
{
    public class EntradaDto
    {
        public string EntradaId { get; set; }
        public DateTime CreationDate { get; set; }
        public string ProviderId { get; set; }
        public Guid ProductIdentity { get; set; }
        public decimal ProductAmount { get; set; }
    }
}