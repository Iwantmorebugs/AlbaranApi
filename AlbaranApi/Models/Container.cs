using System;
using System.ComponentModel.DataAnnotations;

namespace AlbaranApi.Models
{
    public class Container
    {
        public Container(string containerType, int amount, Guid productIdentity)
        {
            ContainerType = containerType;
            Amount = amount;
            ProductIdentity = productIdentity;
        }

        public string ContainerType { get; set; }
        public int Amount { get; set; }

        [Key]
        public Guid ProductIdentity { get; set; }

    }
}
