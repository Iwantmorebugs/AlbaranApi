using System;
using Inventario.EventResult.Contracts;

namespace AlbaranApi.Models
{
    public class EntradaResult : ICommandResultDto, IMessage
    {
        public Guid ExistenciaProductoId { get; set; }
        public string ProductName { get; set; }
        public decimal TotalResult { get; set; }
        public decimal price { get; set; }
        public string Picture { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
    }
}